module.exports = (grunt) ->
  require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks)
  debug = !grunt.option("release")

  grunt.initConfig
    assets: grunt.file.readJSON('assets.json')

    #server
    go:
      options:
        GOPATH: ["../../../../../../../../../../../Users/zack/Dropbox/go"]
      server:
        root: "server"
        output: "app"
        run_files: ["main.go"]

    #client
    connect:
      client:
        options:
          port: 31000
          hostname: 'localhost'
          base:'_dist'
          middleware:  (connect, options) ->
            middlewares = []
            middlewares.push(require('connect-modrewrite')([
              '!\\.html|\\.js|\\.css|\\.otf|\\.eot|\\.svg|\\.ttf|\\.woff|\\.jpg|\\.bmp|\\.gif|\\.png|\\.txt$ /index.html'
            ]))
            require('connect-livereload') port:31003
            options.base.forEach (base) ->
              middlewares.push(connect.static(base))
            return middlewares

    open:
      server:
        url:'http://localhost:31000'

    watch:
      options:
        livereload: 31003
      clientFile:
        files: ['client/**/*','!client/**/*.coffee','!client/**/*.less']
        tasks: ['newer:copy:client','sails-linker','replace:livereload']
      clientCoffee:
        files: ['client/**/*.coffee']
        tasks: ['newer:coffee:client','sails-linker','replace:livereload']

    coffeelint:
      app: ['client/**/*.coffee']
      options:
        max_line_length:
          level: 'ignore'

    coffee:
      options:
        bare: true
      client:
        files: [
          expand: true
          cwd: 'client/'
          src: ['**/*.coffee']
          dest: '_dist/'
          ext: '.js'
        ]

    uglify:
      #options:
        #mangle: false
        #beautify: true
      production:
        files:
          '_dist/index.js': ["<%= assets.js %>"]

    cssmin:
      production:
        files:
          '_dist/index.css': ["<%= assets.css %>"]

    'sails-linker':
      js:
        options:
          startTag: "<!--SCRIPTS-->"
          endTag: "<!--SCRIPTS END-->"
          fileTmpl:  if debug then "<script src='/%s\'><\/script>" else "<script src='/%s?v=#{+new Date()}\'><\/script>"
          appRoot: "_dist/"
        files:
          '_dist/index.html': if debug then ["<%= assets.js %>"] else "_dist/index.js"
      css:
        options:
          startTag: "<!--STYLES-->"
          endTag: "<!--STYLES END-->"
          fileTmpl: if debug then "<link href='/%s' rel='stylesheet' />" else "<link href='/%s?v=#{+new Date()}' rel='stylesheet' />"
          appRoot: "_dist/"
        files:
          '_dist/index.html': if debug then ["<%= assets.css %>"] else "_dist/index.css"

    clean:
      all:
        src: "_dist/**/*"

      redundant:
        src: [
          "_dist/*"
          "!_dist/client/public/data"
          "!_dist/client/public/img"
          "!_dist/client/public/plugin"
          "!_dist/client/public/*.*"
        ]

    copy:
      client:
        files: [
          expand: true
          cwd: 'client/'
          src: [
            '**/*'
            '!**/*.coffee'
          ]
          dest: '_dist'
        ]

    bower:
      client:
        dest: '_dist/vendor'
        options:
          expand: true

    ngtemplates:
      templates:
        src: 'app/**/*.html'
        dest: '_dist/common/templates.js'
        cwd: '_dist'
        options:
          prefix: '/'
          standalone: true
          htmlmin:
            collapseBooleanAttributes:      true
            collapseWhitespace:             true
            removeAttributeQuotes:          true
            removeComments:                 false
            removeEmptyAttributes:          true
            removeRedundantAttributes:      true
            removeScriptTypeAttributes:     true
            removeStyleLinkTypeAttributes:  true

    replace:
      livereload:
        src: ["_dist/index.html"]
        overwrite: true
        replacements: [
          from: '<!--LIVERELOAD-->'
          to: '<script src="//localhost:31003/livereload.js"></script>'
        ]

    concurrent:
      tasks: ['go:run:server', 'watch', 'open']
      options:
        logConcurrentOutput: true


  grunt.registerTask "build", ->
    grunt.task.run [
        "coffeelint"
        "clean:all"
        "bower"
        "copy"
        "coffee"
      ]
    if debug
      grunt.task.run [
        "sails-linker"
        "replace:livereload"
      ]
    else
      grunt.task.run [
        "ngtemplates"
        "uglify"
        "cssmin"
        "sails-linker"
        "clean:redundant"
      ]

  grunt.registerTask "default", [
    'build'
    'connect'
    'concurrent'
  ]
