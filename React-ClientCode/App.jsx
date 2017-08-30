import React from 'react';


class App extends React.Component {
constructor(props) {
     super(props);

     this.state = {
        header: "Header from state...",
        "content": "Content from state..."
     }
  }
   render() {
      return (
         <div>
         <h1>Header</h1>
          <h2>Content</h2>
          <p>This is the content!!!</p>
           <p data-myattribute = "somevalue">This is the content-2!!!</p>
           <h1>{1+1}</h1>
           <Header/>
           <Content/>
           <h2>{this.state.header}</h2>
           <h2>{this.state.content}</h2>
           <h1>test of prop</h1>
           <h2>{this.props.headerProp}</h2>
           <h3>{this.props.contentProp}</h3>
         </div>
      );
   }
}

App.defaultProps = {
   headerProp: "Header from props...",
   contentProp:"Content from props..."
}

class Header extends React.Component {
   render() {
      return (
         <div>
            <h1>Header</h1>
         </div>
      );
   }
}

class Content extends React.Component {
   render() {
      return (
         <div>
            <h2>Content</h2>
            <p>The content text!!!</p>
         </div>
      );
   }
}


export default App;
