import requests

def query_trends(name, query):
    url = 'http://localhost:8080/'
    data = {
        "user": "user",
        "name": name,
        "query": query
    }
    
    response = requests.post(url, json=data)
    response_data = response.json()
    return response_data

while True:
    name = input("Name: ")
    query = input("Query: ")
    response_data = query_trends(name, query)
    print(response_data)