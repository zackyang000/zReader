config = config || {}

config.host =
  domain: 'localhost'
  public: 'localhost:31000'
  api: 'localhost:31001'

config.url =
  public: "http://#{config.host.public}"
  api: "http://#{config.host.api}"

config.site =
  name: 'zReader'
