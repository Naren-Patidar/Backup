var React = require("react");
var utility = require("../utility/common.js");
import classes from '../src/home.css';


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
          mailingnumber:"",
          ddl:"PL",
          url:countryUrl["PL"].toString() + "M80_10500_F.jpg"
      }
    },

    getResponseForMailing:function(res){
    var state = this.state;
    //alert(res[0]);
    //var lis = [];
      //for (var i=0; i < res.length; i++) {
      //alert(res[i]);
      //var str=res[i].split("|");
      //alert(str[0]);
      //    lis.push(<li><img src={ str[1].toString() } height="100" width="100"></img> : response - {str[0]}</li>);
      //}
      //state["mailing"]= lis;
      // alert(res.length);
      state["ImageCount"]= res.length;
      this.setState(state);
       document.getElementById('imgLoader').style.display = "none";
    },

    handleMailingNoChange:function(e){
  //  alert(e.target.name);
      e.preventDefault();
      var mailingNo = e.target.name;
      var state = this.state;
      state[mailingNo] = e.target.value;
      this.setState(state);
    },


    mailingnumber:function(e){
        e.preventDefault();
         document.getElementById('imgLoader').style.display = "block";
        var state = this.state;
        //alert(this.state.mailingnumber);
        utility.checkHttpStatusForMailingNumber(this.state.mailingnumber, this.state.ddl, this.getResponseForMailing);
        this.setState(state);
    },

    handleDDLChange:function(event) {
   this.setState({ddl:event.target.value});

 },



    render:function(){
        return(
           <form className="form">
           <div className={classes.txtMsg}>
           This page is used to verify the presence of coupon images come under the particular mailing period. To test this functionality
           select the country from drop down list and put the mailing number in the in the text-box(like M80, B10).
            <br /><br />
           Once you hit the verify button then application will provide the count of the images present on the web server which all are come under the
            input mailing period.
            <br /><br />
            However this operation is little bit expensive since the possibility of the image count in thousands and verification of response code could take longer time.
           <br /><br /><br />
           <label>
       Select country:&nbsp;&nbsp;&nbsp;&nbsp;
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

                      <label htmlFor="Mailing number">Mailing number: </label>
                      <input type="text" className={classes.txtbox} id="mailingnumber" name="mailingnumber" value={this.state.mailingnumber} onChange={this.handleMailingNoChange} placeholder="Mailing Number" />
                      <br />
                      <br />
           </div>
           <div className={classes.divPositionCenter}>
                      <button type="submit" className={classes.btn} onClick={this.mailingnumber}>Verify images</button>
                       <br />
           </div>
           <div>
                       <br /> <h2 className={classes.txtHeader} >Verification Results:</h2>  <br />
                       <img id="imgLoader" src="./ajax-loader.gif" width="100" height="100" className={classes.displayNone} ></img>
                      <br />
                      <div>Image count : {this.state.ImageCount}</div>
           </div>



            </form>
        )
    }
})
