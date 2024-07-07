from langchain_openai import ChatOpenAI
from langchain_core.prompts import ChatPromptTemplate
from langchain.schema import StrOutputParser
import os

openai_api_key = ""
with open('.env', 'r') as file:
    content = file.read()
    openai_api_key = content.split('=')[1]

def makeScenario():
    chat_llm = ChatOpenAI(openai_api_key=openai_api_key)
    prompt = ChatPromptTemplate.from_messages(
        [
            ( "human", "{question}" ),
        ]
    ) 

    chat_chain = prompt | chat_llm | StrOutputParser()

    response = chat_chain.invoke(
        {
            "question": "Give me a murder scenario in which there are five suspects, one of who has actually committed the crime. Give me details of the victim, activities of the day of the murder for each suspect, and their motives. Give me the name of the murderer along with reason for the murder, but don't conclude the story."
        }
    )

    with open('./data/Scenario.txt', 'w') as file:
        file.write(response)

makeScenario()