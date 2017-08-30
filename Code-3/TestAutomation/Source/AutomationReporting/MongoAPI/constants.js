var CONSTANTS = {};

CONSTANTS.JENKIN_HOST = 'pvcdljenma001uk.global.tesco.org';
CONSTANTS.MSTEST_EXE_PATH = 'C:/Program Files (x86)/Microsoft Visual Studio 10.0/Common7/IDE/MStest.exe';
CONSTANTS.AUTOMATION_ROOT = 'D:/Automation-MCA-MVC/';
CONSTANTS.MONGODB_URL = 'mongodb://172.28.152.12:27017/Automation';
CONSTANTS.TRACE_ENABLE = true;
CONSTANTS.APPLICATION_PORT = 80;
CONSTANTS.RESULT_COLLECTION = 'test_data';
CONSTANTS.STATUS_COLLECTION = 'status_data';
CONSTANTS.ROOT = '/automation';
CONSTANTS.TRIGGER_HOST = 'localhost';
CONSTANTS.TRIGGER_PATH = '/TriggerAutomation/service/run/';
CONSTANTS.TriggerAPI = {
	UK_GD :'172.28.152.7',
	UK_STG: '172.28.152.7',
	CZ_GD: '172.28.152.7',
	CZ_STG: '172.28.152.7',
	HU_GD: '172.28.152.7',
	HU_STG: '172.28.152.7',
	MY_GD: '172.28.152.7',
	MY_STG: '172.28.152.7',
	PL_GD: '172.28.152.7',
	PL_STG: '172.28.152.7',
	SK_GD: '172.28.152.7',
	SK_STG: '172.28.152.7',
	TH_GD: '172.28.152.7',
	TH_STG: '172.28.152.7'
};
CONSTANTS.TriggerAPIPort = {
	UK_GD : 9000,
	UK_STG: 9000,
	CZ_GD: 9000,
	CZ_STG: 9000,
	HU_GD: 9000,
	HU_STG: 9000,
	MY_GD: 9000,
	MY_STG: 9000,
	PL_GD: 9000,
	PL_STG: 9000,
	SK_GD: 9000,
	SK_STG: 9000,
	TH_GD: 9000,
	TH_STG: 9000
};
CONSTANTS.TRIGGERAPI_PATH = '/automation/firetest/batch/';

module.exports = CONSTANTS;