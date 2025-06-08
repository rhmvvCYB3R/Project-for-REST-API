from news import app as application
gunicorn -w 4 -b 0.0.0.0:5000 start:application

