pragma solidity ^0.4.21;

/// @title An example contract title
/// @author Matthew Little
/// @notice This is a test contract
/// @dev Hello dev
contract ExampleContract {

    /// @notice Fake balances variable
    /// @param address auto property getter param
    mapping (address => uint256) public balances;

    /// @notice A test event
    event TestEvent(address indexed _addr, uint64 indexed _id, uint _val);

    string public typeString;
    bytes public typeBytes;
    address public typeAddress;

    bool public typeBool;

    int public typeInt;
    int24 public typeInt24;
    int256 public typeInt256;

    uint public typeUInt;
    uint8 public typeUInt8;
    uint256 public typeUInt256;

	string public givenName;
	bool public enabledThing;
	uint256 public last;

    /// @notice The constructor
    constructor(string _name, bool _enableThing, uint256 _last) public {
		givenName = _name;
		enabledThing = _enableThing;
		last = _last;
    }

	function dynamicArrayInputFunc(int[] dynArr) public returns (bool) {
		return true;
	}

	function staticArrayInputFunc(int[15] staticArr) public returns (bool) {
		return true;
	}

	function getArray() public returns (int16[4]){
		
		int16[4] arr;
		arr[0] = 1;
		arr[1] = -2;
		arr[2] = 29;
		arr[3] = 399;
		return arr;
	}

    /// @author Unknown author
    /// @notice This is a test function
    /// @dev Hi dev
    /// @param _num What number
    /// @return true if _num is 9
    function myFunc(uint256 _num) external pure returns (bool isNine) {
        return _num == 9;
    }

    function tupleTest() public pure returns (int p1, bool p2) {
        return (8, false);
    }

    function overloadedFunc(string _stringParam) {

    }

    function overloadedFunc(int _intParam) {

    }

    function aPrivateFunc() private {

    }

    function examplePayableFunc() public payable {

    }

	function echoString(string val) public returns (string) {
		return val;
	}

	function echoAddress(address val) public returns (address) {
		return val;
	}

	function echoMany(address addr, uint256 num, string str) public returns (address, uint256, string)
	{
		return (addr, num, str);
	}

    /// @notice The fallback function
    function() public {

    }

}