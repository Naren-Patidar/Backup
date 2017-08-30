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

    readFileContent:function(e){
    // alert(document.getElementById('imgLoader'));
     document.getElementById('imgLoader').style.display = "block";
      var file = e.target.files[0];
      var reader = new FileReader();
       reader.onload = function(evt){
            var resultText = evt.target.result;
            // alert(resultText);
            utility.ImageVarification(resultText, this.state.ddl, this.getImageAvailStatus);
       }.bind(this);

       reader.readAsText(file);
    },
    getImageAvailStatus:function(response){
    //alert(response);
    var res = response;
    this.setState({"response":response});
    document.getElementById('imgLoader').style.display = "none";
    document.getElementById('downloadResult').style.display = "block";
    },

    verifyImages:function(e){

     document.getElementById('imgLoader').style.display = "block";
      var file =  document.getElementById('fileSelector').files[0];

      var reader = new FileReader();
       reader.onload = function(evt){
            var resultText = evt.target.result;
            utility.ImageVarification(resultText, this.state.ddl, this.getImageAvailStatus);
       }.bind(this);

       reader.readAsText(file);
    },

    downloadResultInFile:function(e){
    document.getElementById('download').click();
    },


    handleDDLChange:function(event) {
   this.setState({ddl:event.target.value});
 },



    render:function(){
        return(
           <form className="form">
           <div className={classes.txtMsg}>
           This page is used the validate the coupon image presence on the web server.
           Here you can provide the bulk image names and verify the result.   <br />
           <br />
           Use the text file as a input of this functionality. The text file should have image names separated with the new
           line character. Once the verify button is pressed then application will give you list of the images which all are
           not present on the web server. You can also download the result set into the text file by hitting the download button
           present in the verification section.
           <br />
           <br /><br />
           <div className={classes.positionRight}>
           <a href="sampleData.txt" download id="downloadSampleFile">download sample input file</a>
           </div>
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
           <br />
                <div className={classes.txtMsg}>
                  <input type="file" id="fileSelector"></input>

                </div>
                <br />
                <br />
                <div  className={classes.divPositionCenter}>
                    <button className={classes.btn} type="submit" onClick={this.verifyImages}>Verify images</button>
                </div>
                <br />
                <br />
                <div className={classes.txtHeader}>
                  Verification result
                  </div>
                  <br /><br />

                <div className={classes.txtMsg}>
                Images not present on server : <label className={classes.resultContent}>{this.state.response}</label>
                <img id="imgLoader" src="./ajax-loader.gif" width="100" height="100" className={classes.displayNone}></img>
                <br /><br />
                <div id="downloadResult" className={classes.displayNone}>
                <button className={classes.btnDownload} type="submit" onClick={this.downloadResultInFile}>Download result in file</button>
                </div>
                </div>
                <a href="imgNotPresentOnServer.txt" download id="download" hidden></a>
            </form>
        )
    }
})
