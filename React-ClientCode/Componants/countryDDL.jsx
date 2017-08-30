var React = require("react");
import classes from '../src/home.css';

let styles = {
  container: {
    borderRadius: 4,
    borderWidth: 0.5,
    borderColor: '#d6d7da',
  },
  title: {
    fontSize: 19,
    fontWeight: 'bold',
  },
  activeTitle: {
    color: 'red',
  },
}

module.exports = React.createClass({
    getInitialState:function(){
      return {
          ddl:"PL"
      }
    },
  handleDDLChange:function(event) {
   this.setState({ddl:event.target.value});
   alert(event.target.value);
 },
    render:function(){
        return(
               <div style={styles.container}>
               <div style={classes.headerText}>Hi every body</div>
                     <label style={styles.title}>
                         Select country:
                           <select value={this.state.ddl} onChange={this.handleDDLChange}>
                             <option value="PL">Poland</option>
                             <option value="CZ">Czech</option>
                             <option value="HU">hungary</option>
                             <option value="SL">slovakia</option>
                           </select>
                   </label>
               </div>
        )
    }
})
