import React, { Component } from 'react'
import { Router, Route, Link, IndexRoute, hashHistory, browserHistory } from 'react-router'
import App3 from './app3.jsx';
import App2 from './app2.jsx';
import withImageName from './Componants/verifyWithImageName.jsx';
import withMailingNo from './Componants/verifyWithMailingNo.jsx';
import withImageList from './Componants/verifyWithListOfImage.jsx';
import ddl from './Componants/countryDDL.jsx';
import classes from './src/home.css';



class App extends Component {
  render () {
    return (
      <Router history={hashHistory}>
        <Route path='/' component={Container}>
          <IndexRoute component={withImageName} />
          <Route path='WithImageName' component={withImageName} />
          <Route path='WithMailingNo' component={withMailingNo} />
          <Route path='WithImageList' component={withImageList} />
          <Route path='*' component={NotFound} />
        </Route>
      </Router>
    )
  }
}
const Home = () => <h1>Hello from Home!</h1>
const Address = () => <h1>We are located at 555 Jackson St.</h1>
const NotFound = () => <h1>404.. This page is not found!</h1>

const Nav = () => (
  <div>
    Verify with :&nbsp;&nbsp;&nbsp;
    <Link activeStyle={{color:'#FF9800'}} className={classes.anchor}  to='/WithImageName'>Image name  </Link>&nbsp;| &nbsp;
    <Link activeStyle={{color:'#FF9800'}} className={classes.anchor}  to='/WithMailingNo'>Mailing number  </Link>&nbsp;| &nbsp;
    <Link activeStyle={{color:'#FF9800'}} className={classes.anchor}  to='/WithImageList'>Image list</Link>&nbsp;&nbsp;
  </div>
)

const Container = (props) => <div className={classes.container}>
<div className={classes.menuDiv}>
<div className={classes.right}><Nav /></div>
</div>
<br />
<div className={classes.content}>
{props.children}
</div>
</div>

export default App
