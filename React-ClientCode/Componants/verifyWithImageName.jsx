var React = require("react");
import classes from '../src/home.css';
var utility = require("../utility/common.js");

const countryUrl = {
  PL: "https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/",
  CZ: "https://zabezpeceni.itesco.cz/MojeClubcard/Ucet/I/CouponImages/",
  SK: "https://s.itesco.sk/mojaclubcard/ucet/I/couponimages/",
  HU: "https://s.tesco.hu/clubcardfiokom/fiokom/I/couponimages/"
}

module.exports = React.createClass({
    getInitialState:function(){
      return {
          name:"",
          ddl:"PL",
          url:countryUrl["PL"].toString() + "M80_10500_F.jpg"
      }
    },
    checkImages:function(e){
        e.preventDefault();
        document.getElementById('imgLoader').style.display = "block";
        var state = this.state;
        // alert(this.state.name);
        // alert(state.ddl);
        //alert(countryUrl[this.state.ddl].toString());
        state["url"] = countryUrl[this.state.ddl].toString() + this.state.name;
        utility.checkHttpStatus(this.state.name, this.state.ddl, this.setResponse);
        this.setState(state);
    },

    setResponse:function(res){
    //alert(res);
    var arr = res.split("|");
    var state = this.state;
    state["response"]=arr[0];
    this.setState(state);
    document.getElementById('imgLoader').style.display = "none";
    },

    handleInputChange:function(e){
    //alert(e.target.name);
      e.preventDefault();
      var name = e.target.name;
      var state = this.state;
      state[name] = e.target.value;
      this.setState(state);
    },
    handleDDLChange:function(event) {
     this.setState({ddl:event.target.value});
     //alert(event.target.value);
   },

    render:function(){
        return(
           <form className="form">

           <div className={classes.txtMsg}>
             This page is used to verify the presence of coupon image on the web server. To test this functionality
             select the country from drop down list and put the full image name with the extention in the textbox.
              <br /><br />
             Once you hit the verification button then application provide you the verification result like http response code, image path,
             image url, and image container.
             <br /><br /><br />
                 <label>

                    Select country:
                       <select value={this.state.ddl} className={classes.ddlSelect} onChange={this.handleDDLChange}>
                         <option value="PL">Poland</option>
                         <option value="CZ">Czech</option>
                         <option value="HU">hungary</option>
                         <option value="SL">slovakia</option>
                       </select>
               </label>
           </div>
           <br />

                <div className={classes.txtMsg}>
                <br />
                    <label className="control-label" htmlFor="Image Name">Image Name: &nbsp;&nbsp;</label>
                    <input type="text" className={classes.txtbox} id="name" name="name" value={this.state.name} onChange={this.handleInputChange} placeholder="Image Name" />
                </div>
                <br />
                <br />
                <div  className={classes.divPositionCenter}>
                    <button className={classes.btn} type="submit" onClick={this.checkImages}>Verify image</button>
                </div>
                <br />
                  <img id="imgLoader" src="./ajax-loader.gif" width="100" height="100" className={classes.displayNone}></img>
                <div>
                <h2 className={classes.txtHeader}>Verification results</h2>
                    <div className={classes.txtMsg}>
                       Image name :  &nbsp; {this.state.name}<br /><br />
                       Image url:  &nbsp; {this.state.url}<br /><br />
                       Response code : {this.state.response} <br /><br />
                    </div>
                    <div>
                       <img src={this.state.url} alt="" height="100" width="100"/>
                    </div>
                </div>

            </form>
        )
    }
})
