﻿pragma solidity ^0.4.21;

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

    bool[] public typeArrayDynamic;
    bool[10] public typeArrayFixed;
    bool[2][5] public typeArray2DimFixed;
    byte[] public typeByteArrayDynamic;
	bytes22 public typeByteArrayFixed;

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

    /// @notice The constructor
    /// @param _name Name param
    constructor(string _name) public {

    }

	function dynamicArrayInputFunc(int[] dynArr) public returns (bool) {
		return true;
	}

	function staticArrayInputFunc(int[15] staticArr) public returns (bool) {
		return true;
	}

    /// @author Unknown author
    /// @notice This is a test function
    /// @dev Hi dev
    /// @param _num What number
    /// @return true if _num is 9
    function myFunc(int256 _num) external pure returns (bool isNine) {
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

    /// @notice The fallback function
    function() public {

    }

}