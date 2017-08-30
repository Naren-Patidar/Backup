import React from 'react';
var common  = require('./server/util.js');

class App2 extends React.Component {
   constructor(props) {
      super(props);

      this.state = {
         header: "Header from props...",
         "content": "Content from props...",
         xxx: this.getRes.bind(this)()
      }
   }

  getRes(){
  //return common.getReponse('M80_10500_T.jpg');
  //return common.getReponse('M80_10500_T.jpg');
  var a;
  a = common.createURL('M80_10500_T.jpg');
  return a;
  }

   render() {
   var x = 'M80_10500_T.jpg' ;
   console.log('test');
   var y = common.createURL(x);
   //var z = common.getReponse(y);

      return (
         <div>
            <Header headerProp ={this.state.xxx}/>
            <Content contentProp = {this.state.content}/>
            <Input/>
         </div>
      );
   }
}

class Input extends React.Component {
   render() {
      return (
         <div>
        <h2>Please provide the image name to verification : </h2>
          <input type="text" id="name" name="name" placeholder="School Name" />
         <button type="submit">Test Image</button>
         </div>
      );
   }
}

class Header extends React.Component {
   render() {
      return (
         <div>
            <h1>{this.props.headerProp}</h1>
         </div>
      );
   }
}

class Content extends React.Component {
   render() {
      return (
         <div>
            <h2>{this.props.contentProp}</h2>
         </div>
      );
   }
}

export default App2;
