from news import app as application
gunicorn start:application --bind 0.0.0.0:5000

