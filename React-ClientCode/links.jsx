import React from 'react';
import ReactDOM from 'react-dom';
import { Router, Route, Link, IndexRoute, hashHistory, browserHistory } from 'react-router'

const Nav = () => (
  <div>
    <Link to='/'>Home</Link>&nbsp;<br />
    <Link to='/address'>Address</Link>
  </div>
)

const Container = (props) => <div>
<table>
<tr>
<td><Nav /></td>
<td>{props.children}</td>
</tr>
</table>


</div>

export default Container;
