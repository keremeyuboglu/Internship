FROM elasticsearch:7.14.0
HEALTHCHECK --interval=5s --timeout=3s CMD curl -f -X GET "localhost:9200/_cluster/health?wait_for_status=green&timeout=1s" || exit 1