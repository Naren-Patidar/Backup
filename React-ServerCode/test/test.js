var expect = require('chai').expect
var name = 'React Quickly'
var url = ['http://reactquickly.co', 'https://www.manning.com/books/react-quickly']


describe('name and url', function() {
  it('must match the values', function(done){
    expect(name).to.be.a('string')
    expect(name).to.equal('React Quickly')
    expect(url).to.have.length(2)
    done()
  })
});

describe('Image url contain JPG images', function() {
  it('must contain JPG', function(done){
    expect('https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/M80_10500_T.jpg').to.contain('jpg');
    done()
  })
});
describe('URL should not empty string', function() {
  it('Not null', function(done){
    var url='https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/M80_10500_T.jpg';
    expect(url).to.not.be.empty;
    done()
  })
});
describe('Array should not empty', function() {
  it('Not empty', function(done){
    var arr=[1,2,3];
    expect(arr).to.not.be.empty;
    done()
  })
});
describe('Mailing Number', function() {
  it('Should not empty', function(done){
    var mailingNo='m80';
    expect(mailingNo).to.not.be.empty;
    done()
  })
});
describe('Mailing Number length validation', function() {
  it('Should more than 2', function(done){
    var mailingNo='m80';
    expect(mailingNo).to.have.length.above(2);
    done()
  })
});
describe('URL contain http or https', function() {
  it('Shoud contain http or https', function(done){
    var url='https://s.tesco.pl/Clubcard/mojekonto/I/couponimages/M80_10500_T.jpg';
    expect(url).to.not.be.empty;
    expect(url).to.contain('http')
    done()
  })
});

describe('Request should not empty', function() {
  it('Shoud not null', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('http')
    done()
  })
});

describe('Request object have parametr', function() {
  it('Shoud have atleast one parameter', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('search')
    done()
  })
});
describe('Response should not empty', function() {
  it('Shoud not null', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('http')
    done()
  })
});

describe('Response object have data', function() {
  it('Shoud have atleast one parameter', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('search')
    done()
  })
});
describe('Function should return some value', function() {
  it('return value is string', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('country')
    done()
  })
});
describe('Function', function() {
  it('return value is integer', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('country')
    done()
  })
});
describe('callback function', function() {
  it('Wait to execute the callback and return value', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('country')
    done()
  })
});
describe('Data boxing', function() {
  it('Box integer to string', function(done){
    var url='http://127.0.0.1:8081/search/mn/m80/country/PL';
    expect(url).to.not.be.empty;
    expect(url).to.contain('country')
    done()
  })
});

var assert = require('assert');
describe('Array', function() {
  describe('#indexOf()', function() {
    it('should return -1 when the value is not present', function() {
      assert.equal(-1, [1,2,3].indexOf(4));
    });
  });
});

function add() {
  return Array.prototype.slice.call(arguments).reduce(function(prev, curr) {
    return prev + curr;
  }, 0);
}

describe('Request object()', function() {
  var tests = [
    {args: [1, 2],       expected: 3},
    {args: [1, 2, 3],    expected: 6},
    {args: [1, 2, 3, 4], expected: 10},
    {args: [1, 2, 3, 4, 5], expected: 15}
  ];

  tests.forEach(function(test) {
    it('correctly adds ' + test.args.length + ' args', function() {
      var res = add.apply(null, test.args);
      assert.equal(res, test.expected);
    });
  });
});

describe('Array', function() {
  it('should start empty', function() {
    var arr = [];

    assert.equal(arr.length, 0);
  });
});

describe('addClass', function() {
  it('should add class to element', function() {
    var element = { className: '' };

    addClass(element, 'test-class');

    assert.equal(element.className, 'test-class');
  });

  it('should not add a class which already exists');
});
