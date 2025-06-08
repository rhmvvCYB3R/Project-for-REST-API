from flask import Flask, jsonify
import requests
from flask_cors import CORS 

app = Flask(__name__)
CORS(app)

NEWS_API_KEY = '6de84ff626914426be310563b0a7b4a9'
NEWS_API_URL = 'https://newsapi.org/v2/top-headlines'

@app.route('/api/news')
def get_news():
    params = {
        'category': 'business',
        'language': 'en',
        'apiKey': NEWS_API_KEY,
        'pageSize': 10
    }
    response = requests.get(NEWS_API_URL, params=params)
    data = response.json()

    if data.get('status') != 'ok':
        return jsonify({'error': 'Failed to fetch news'}), 500

    articles = [{
        'title': article['title'],
        'url': article['url'],
        'source': article['source']['name'],
        'publishedAt': article['publishedAt']
    } for article in data['articles']]

    return jsonify({'articles': articles})

if __name__ == '__main__':
    app.run(port=5000, debug=True)
