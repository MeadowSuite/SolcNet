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

    bool[] public typeArrayDynamic;
    bool[10] public typeArrayFixed;
    bool[2][5] public typeArray2DimFixed;
    byte[] public typeByteArrayDynamic;

    string public typeString;
    bytes public typeBytes;
    address public typeAddress;

    bool public typeBool;

    int public typeInt;
    int8 public typeInt8;
    int16 public typeInt16;
    int24 public typeInt24;
    int32 public typeInt32;
    int40 public typeInt40;
    int48 public typeInt48;
    int56 public typeInt56;
    int64 public typeInt64;
    int72 public typeInt72;
    int80 public typeInt80;
    int88 public typeInt88;
    int96 public typeInt96;
    int104 public typeInt104;
    int112 public typeInt112;
    int120 public typeInt120;
    int128 public typeInt128;
    int136 public typeInt136;
    int144 public typeInt144;
    int152 public typeInt152;
    int160 public typeInt160;
    int168 public typeInt168;
    int176 public typeInt176;
    int184 public typeInt184;
    int192 public typeInt192;
    int200 public typeInt200;
    int208 public typeInt208;
    int216 public typeInt216;
    int224 public typeInt224;
    int232 public typeInt232;
    int240 public typeInt240;
    int248 public typeInt248;
    int256 public typeInt256;

    uint public typeUInt;
    uint8 public typeUInt8;
    uint16 public typeUInt16;
    uint24 public typeUInt24;
    uint32 public typeUInt32;
    uint40 public typeUInt40;
    uint48 public typeUInt48;
    uint56 public typeUInt56;
    uint64 public typeUInt64;
    uint72 public typeUInt72;
    uint80 public typeUInt80;
    uint88 public typeUInt88;
    uint96 public typeUInt96;
    uint104 public typeUInt104;
    uint112 public typeUInt112;
    uint120 public typeUInt120;
    uint128 public typeUInt128;
    uint136 public typeUInt136;
    uint144 public typeUInt144;
    uint152 public typeUInt152;
    uint160 public typeUInt160;
    uint168 public typeUInt168;
    uint176 public typeUInt176;
    uint184 public typeUInt184;
    uint192 public typeUInt192;
    uint200 public typeUInt200;
    uint208 public typeUInt208;
    uint216 public typeUInt216;
    uint224 public typeUInt224;
    uint232 public typeUInt232;
    uint240 public typeUInt240;
    uint248 public typeUInt248;
    uint256 public typeUInt256;

    /// @notice The constructor
    /// @param _name Name param
    constructor(string _name) public {

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