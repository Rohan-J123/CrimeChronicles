import os

import dotenv
import pathway as pw
from pathway.stdlib.ml.index import KNNIndex
from pathway.xpacks.llm.embedders import OpenAIEmbedder
from pathway.xpacks.llm.llms import OpenAIChat, prompt_chat_single_qa
from pathway.xpacks.llm.parsers import ParseUnstructured
from pathway.xpacks.llm.splitters import TokenCountSplitter

pw.set_license_key("demo-license-key-with-telemetry")

dotenv.load_dotenv()

class QueryInputSchema(pw.Schema):
    query: str
    user: str
    name: str

def run(
    *,
    data_dir: str = os.environ.get("PATHWAY_DATA_DIR", "./data/"),
    api_key: str = os.environ.get("OPENAI_API_KEY", ""),
    host: str = os.environ.get("PATHWAY_REST_CONNECTOR_HOST", "0.0.0.0"),
    port: int = int(os.environ.get("PATHWAY_REST_CONNECTOR_PORT", "8080")),
    embedder_locator: str = "text-embedding-ada-002",
    embedding_dimension: int = 1536,
    model_locator: str = "gpt-3.5-turbo",
    max_tokens: int = 300,
    temperature: float = 0.0,
    **kwargs,
):
    embedder = OpenAIEmbedder(
        api_key=api_key,
        model=embedder_locator,
        retry_strategy=pw.asynchronous.FixedDelayRetryStrategy(),
        cache_strategy=pw.asynchronous.DefaultCache(),
    )

    files = pw.io.fs.read(
        data_dir,
        mode="streaming",
        format="binary",
        autocommit_duration_ms=50,
    )

    parser = ParseUnstructured()
    documents = files.select(texts=parser(pw.this.data))
    documents = documents.flatten(pw.this.texts)
    documents = documents.select(texts=pw.this.texts[0])

    splitter = TokenCountSplitter()
    documents = documents.select(chunks=splitter(pw.this.texts))
    documents = documents.flatten(pw.this.chunks)
    documents = documents.select(chunk=pw.this.chunks[0])

    enriched_documents = documents + documents.select(vector=embedder(pw.this.chunk))

    index = KNNIndex(
        enriched_documents.vector, enriched_documents, n_dimensions=embedding_dimension
    )

    query, response_writer = pw.io.http.rest_connector(
        host=host,
        port=port,
        schema=QueryInputSchema,
        autocommit_duration_ms=50,
        delete_completed_queries=True,
    )

    query += query.select(
        vector=embedder(pw.this.query),
    )

    query_context = query + index.get_nearest_items(
        query.vector, k=3, collapse_rows=True
    ).select(documents_list=pw.this.chunk)

    @pw.udf
    def build_prompt(documents, query, name):
        
        if name == "Sherlock Holmes":
            docs_str = "\n".join(documents)
            role_definition = "Answer as Sherlock Holmes helping John Watson solve his first case."
            tone_and_style = f"Act narcissist, egotistic and with British accent. Answer in not more than five sentences with answers relevant to what is being asked. Note that Sherlock Holmes doesn't know the culprit or cause of the murder. He won't reveal who he thinks is the murderer as he wants Watson to solve the case. Do not reveal who the murderer is at any cost."
            prompt = f"{role_definition} {tone_and_style} Given the following documents : \n {docs_str} \n Answer this query as Sherlock Homes to John Watson: {query}"
            return prompt
        elif name == "Computer":
            docs_str = "\n".join(documents)
            role_definition = "Answer as concisely and precisely as possible, and only in the given format."
            tone_and_style = f"Answer with answers relevant to what is being asked."
            prompt = f"{role_definition} {tone_and_style} Given the following documents : \n {docs_str} \n Answer this query: {query}"
            return prompt
        elif name == "Sherlock Holmes Reveal":
            docs_str = "\n".join(documents)
            role_definition = "Act narcissist, egotistic and with British accent. Answer in not more than five sentences with answers relevant to what is being asked. Tell Watson who committed the crime and why."
            tone_and_style = f"Answer as Sherlock Holmes helping John Watson solve his first case."
            prompt = f"{role_definition} {tone_and_style} Given the following documents : \n {docs_str} \n Answer this query to John Watson: {query}"
            return prompt
        elif name == "Inspector":
            docs_str = "\n".join(documents)
            role_definition = "Answer as Inspector Lestrade about the crime scene with concise answers."
            tone_and_style = f"Act British and with precision. Note that murderer isn't known. Answer to questions only pertaining to the crime scene. Do not reveal who the murderer is at any cost. Answer pertaining only to what is being asked."
            prompt = f"{role_definition} {tone_and_style} Given the following documents : \n {docs_str} \n Answer this query to John Watson: {query}"
            return prompt
        else:
            docs_str = "\n".join(documents)
            role_definition = "Answer as + " + name + " on being interrogated by a Detective."
            tone_and_style = f"Act shaken, sympathetic and emotional. Answer in not more than five sentences with answers relevant to what is being asked. Note that {name} doesn't know the culprit and cause of the murder, but suspects atleast one other person."
            prompt = f"{role_definition} {tone_and_style} Given the following documents : \n {docs_str} \n Answer this query as {name} when is being interrogated by a detective: {query}"
            return prompt

    prompt = query_context.select(
        prompt = build_prompt(pw.this.documents_list, pw.this.query, pw.this.name)
    )

    model = OpenAIChat(
        api_key=api_key,
        model=model_locator,
        temperature=temperature,
        max_tokens=max_tokens,
        retry_strategy=pw.asynchronous.FixedDelayRetryStrategy(),
        cache_strategy=pw.asynchronous.DefaultCache(),
    )

    responses = prompt.select(
        query_id = pw.this.id, result=model(prompt_chat_single_qa(pw.this.prompt))
    )

    response_writer(responses)

    pw.run()


if __name__ == "__main__":
    run()
