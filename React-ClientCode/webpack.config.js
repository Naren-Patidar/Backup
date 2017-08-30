

var config = {
   entry: './main.js',

   output: {
      path:__dirname + './',
      filename: 'index.js',
   },

   devServer: {
      inline: true,
      port: 8000
   },
   resolve: {
     modules: [
      'node_modules' ]
 },

   module: {
      loaders: [
         {
            test: /\.jsx?$/,
            exclude: /node_modules/,
            loader: 'babel-loader',

            query: {
               presets: ['es2015', 'react']
            }
         },
        {
          test: /\.css$/,
          loader: 'style-loader'
        },
        {
          test: /\.css$/,
          loader: 'css-loader',          
          query: {
            modules: true,
            localIdentName: '[name]__[local]___[hash:base64:5]'
          }
      },
      { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' }
      ]
   }
}

module.exports = config;
