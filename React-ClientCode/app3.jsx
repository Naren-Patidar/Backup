var React = require("react");
var utility = require("./utility/common.js");

const countryUrl = {
  PL: "https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/",
  CZ: "https://zabezpeceni.itesco.cz/MojeClubcard/Ucet/I/CouponImages/",
  SK: "print",
  HU: "Hungary"
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
    checkImages:function(e){
        e.preventDefault();
        var state = this.state;
        //alert(countryUrl[this.state.ddl].toString());
        state["url"] = countryUrl[this.state.ddl].toString() + this.state.name;
        utility.checkHttpStatus(this.state.name, this.state.ddl, this.setResponse);
        this.setState(state);
      //  document.getElementById('download').click();


    },
    setResponse:function(res){
    var state = this.state;
    state["response"]=res;
    this.setState(state);
    },
    getResponseForMailing:function(res){
    var state = this.state;
  //  alert(res[0]);
    var lis = [];

      for (var i=0; i < res.length; i++) {
      //alert(res[i]);
      var str=res[i].split("|");
      //alert(str[0]);
          lis.push(<li><img src={ str[1].toString() } height="100" width="100"></img> : response - {str[0]}</li>);
      }
      state["mailing"]= lis;
      alert(res.length);
      state["ImageCount"]= res.length;
      this.setState(state);
    },
    handleInputChange:function(e){
    //alert(e.target.name);
      e.preventDefault();
      var name = e.target.name;
      var state = this.state;
      state[name] = e.target.value;
      this.setState(state);
    },

    handleMailingNoChange:function(e){
  //  alert(e.target.name);
      e.preventDefault();
      var mailingNo = e.target.name;
      var state = this.state;
      state[mailingNo] = e.target.value;
      this.setState(state);
    },
    readFileContent:function(e){
      var file = e.target.files[0];
      var reader = new FileReader();
       reader.onload = function(evt){
            var resultText = evt.target.result;
            alert(resultText);
            utility.ImageVarification(resultText, this.state.ddl, this.getImageAvailStatus);
       }.bind(this);
       reader.readAsText(file);
    },
    getImageAvailStatus:function(response){
    alert(response);
    },

    mailingnumber:function(e){
        e.preventDefault();
        var state = this.state;
        alert(this.state.mailingnumber);
        utility.checkHttpStatusForMailingNumber(this.state.mailingnumber, this.state.ddl, this.getResponseForMailing);
        this.setState(state);
    },

    handleDDLChange:function(event) {
   this.setState({ddl:event.target.value});
  // alert(event.target.value);
 },



    render:function(){
        return(
           <form className="form">
           <div>
           <label>
       Select country:
       <select value={this.state.ddl} onChange={this.handleDDLChange}>
         <option value="PL">Poland</option>
         <option value="CZ">Czech</option>
         <option value="HU">hungary</option>
         <option value="SL">slovakia</option>
       </select>
     </label>
           </div>
                <div className="form-group">
                <div>Extra div : {this.state.name}</div>
                <h2>Please enter the full image name with the extention</h2>
                    <label className="control-label" htmlFor="Image Name">Image Name:</label>
                    <input type="text" className="form-control" id="name" name="name" value={this.state.name} onChange={this.handleInputChange} placeholder="Image Name" />
                </div>
                <div className="form-group">
                    <button className="btn" type="submit" onClick={this.checkImages}>Check image</button>
                </div>
                <div>
                <h2>Result</h2>
                    <div>
                       <img src={this.state.url} alt="" height="100" width="100"/>
                    </div>
                </div>
                <div>
                      <h2>Please enter mailing number</h2>
                    <div>Extra div : {this.state.mailingnumber}</div>
                      <label className="control-label" htmlFor="Mailing Number">Mailing Number:</label>
                      <input type="text" className="form-control" id="mailingnumber" name="mailingnumber" value={this.state.mailingnumber} onChange={this.handleMailingNoChange} placeholder="Mailing Number" />
                      <button className="btn" type="submit" onClick={this.mailingnumber}>Check images for mailing number</button>
                      <div>Image count : {this.state.ImageCount}</div>
                      <div>Extra div 3 : {this.state.mailing}</div>

                      <a href="coupon.pdf" download id="download" hidden></a>
                </div>
                <div>
                  <input type="file" onChange={this.readFileContent}></input>
                </div>
            </form>
        )
    }
})
