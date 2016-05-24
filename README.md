# gchandle-fatalrepro
Repro project for a GCHandle related ExecutionEngineException

# Exception Details

INVALID_POINTER_WRITE in clr!TableFreeSingleHandleToCache possibly @ https://github.com/dotnet/coreclr/blob/master/src/gc/handletablecache.cpp#L800

```
(1b68.227c): Access violation - code c0000005 (first chance)
First chance exceptions are reported before any exception handling.
This exception may be expected and handled.
eax=fb3ed968 ebx=fb3cdb68 ecx=ffffffff edx=000000ff esi=f5de3cf4 edi=000000ff
eip=72bb883b esp=0018efdc ebp=0018efe8 iopl=0         nv up ei ng nz na po nc
cs=0023  ss=002b  ds=002b  es=002b  fs=0053  gs=002b             efl=00010282
clr!TableFreeSingleHandleToCache+0x3f:
72bb883b f00fc18890020000 lock xadd dword ptr [eax+290h],ecx ds:002b:fb3edbf8=????????
0:000> !analyze -v
*******************************************************************************
*                                                                             *
*                        Exception Analysis                                   *
*                                                                             *
*******************************************************************************

*** WARNING: Unable to verify checksum for GCHandleExecutionEngineExceptionRepro.exe

DUMP_CLASS: 2

DUMP_QUALIFIER: 0

FAULTING_IP: 
clr!TableFreeSingleHandleToCache+3f
72bb883b f00fc18890020000 lock xadd dword ptr [eax+290h],ecx

EXCEPTION_RECORD:  (.exr -1)
ExceptionAddress: 72bb883b (clr!TableFreeSingleHandleToCache+0x0000003f)
   ExceptionCode: c0000005 (Access violation)
  ExceptionFlags: 00000000
NumberParameters: 2
   Parameter[0]: 00000001
   Parameter[1]: fb3edbf8
Attempt to write to address fb3edbf8

FAULTING_THREAD:  0000227c

PROCESS_NAME:  GCHandleExecutionEngineExceptionRepro.exe

ERROR_CODE: (NTSTATUS) 0xc0000005 - The instruction at 0x%p referenced memory at 0x%p. The memory could not be %s.

EXCEPTION_CODE: (NTSTATUS) 0xc0000005 - The instruction at 0x%p referenced memory at 0x%p. The memory could not be %s.

EXCEPTION_CODE_STR:  c0000005

EXCEPTION_PARAMETER1:  00000001

EXCEPTION_PARAMETER2:  fb3edbf8

WRITE_ADDRESS:  fb3edbf8 

FOLLOWUP_IP: 
clr!TableFreeSingleHandleToCache+3f
72bb883b f00fc18890020000 lock xadd dword ptr [eax+290h],ecx

WATSON_BKT_PROCSTAMP:  574426eb

WATSON_BKT_PROCVER:  1.0.0.0

PROCESS_VER_PRODUCT:  GCHandleExecutionEngineExceptionRepro

WATSON_BKT_MODULE:  clr.dll

WATSON_BKT_MODSTAMP:  56e1f25a

WATSON_BKT_MODOFFSET:  d883b

WATSON_BKT_MODVER:  4.6.1078.0

MODULE_VER_PRODUCT:  Microsoft® .NET Framework

BUILD_VERSION_STRING:  10.0.10586.162 (th2_release_sec.160223-1728)

MODLIST_WITH_TSCHKSUM_HASH:  028f34ea1d45968c0ee4f8d19ae6e4c290120b29

MODLIST_SHA1_HASH:  2782cc683f95f99ef3f51ab8d931ccbc5221251e

NTGLOBALFLAG:  2000100

APPLICATION_VERIFIER_FLAGS:  81643027

PRODUCT_TYPE:  1

SUITE_MASK:  272

APPLICATION_VERIFIER_LOADED: 1

APP:  gchandleexecutionengineexceptionrepro.exe

MISSING_CLR_SYMBOL: 0

ANALYSIS_SESSION_HOST:  NOGHE

ANALYSIS_SESSION_TIME:  05-24-2016 13:06:55.0757

ANALYSIS_VERSION: 10.0.10586.567 x86fre

MANAGED_CODE: 1

MANAGED_ENGINE_MODULE:  clr

MANAGED_ANALYSIS_PROVIDER:  SOS

THREAD_ATTRIBUTES: 
OS_LOCALE:  ENG

PROBLEM_CLASSES: 



AVRF
    Tid    [0x227c]
    Frame  [0x00]: clr!TableFreeSingleHandleToCache
    Failure Bucketing



INVALID_POINTER_WRITE
    Tid    [0x227c]
    Frame  [0x00]: clr!TableFreeSingleHandleToCache


BUGCHECK_STR:  INVALID_POINTER_WRITE_AVRF

DEFAULT_BUCKET_ID:  INVALID_POINTER_WRITE_AVRF

LAST_CONTROL_TRANSFER:  from 72bb88de to 72bb883b

STACK_TEXT:  
0018efe8 72bb88de f5de3cf4 bda62100 f5de3cf4 clr!TableFreeSingleHandleToCache+0x3f
0018f024 72bb891e f5de3cf4 000000ff 72b1287a clr!HndDestroyHandle+0x133
0018f030 72b1287a bda63e40 0018f12c f5de3cf4 clr!DestroyTypedHandle+0x1c
0018f0b8 713b67de 0018f39c f84c8a9c 0018f130 clr!MarshalNative::GCHandleInternalFree+0x72
0018f0cc f6544df9 f8488d94 f8480294 00000000 mscorlib_ni+0x3167de
WARNING: Frame IP not in any known module. Following frames may be wrong.
0018f144 f6541c65 00000000 00000000 00000000 0xf6544df9
0018f2c4 f6540998 00000000 f84328bc 00002710 0xf6541c65
0018f2ec f6540464 f84328a4 0018f304 72ae1376 0xf6540998
0018f2f8 72ae1376 fa5b5ac0 0018f358 72ae366f 0xf6540464
0018f304 72ae366f 0018f39c 0018f348 72bea0ca clr!CallDescrWorkerInternal+0x34
0018f358 72aed376 0018f3b0 f8433258 00000000 clr!CallDescrWorkerWithHandler+0x6b
0018f3c8 72bfa2b4 0018f4c4 bda63a14 faf94e4c clr!MethodDescCallSite::CallTargetWorker+0x158
0018f4ec 72bfa4bb f84328a4 00000000 bda63bf8 clr!RunMain+0x1aa
0018f760 72bfaae3 00000000 bda63988 00b50000 clr!Assembly::ExecuteMainMethod+0x124
0018fc68 72bfab89 bda63238 00000000 00000000 clr!SystemDomain::ExecuteMainMethod+0x651
0018fcc0 72bfa38a bda633f8 00000000 00000000 clr!ExecuteEXE+0x4c
0018fd00 72c15a47 bda633c4 00000000 00000000 clr!_CorExeMainInternal+0xdc
0018fd3c 7381cccb 72caf5c3 7398dc60 7381cc5a clr!_CorExeMain+0x4d
0018fd78 7398dd05 7398dc60 cb6c1a02 0018fd9c mscoreei!_CorExeMain+0x10a
0018fd88 74af38f4 002f1000 74af38d0 2c47aafd MSCOREE!_CorExeMain_Exported+0xa5
0018fd9c 76fa5de3 002f1000 cf74a662 00000000 KERNEL32!BaseThreadInitThunk+0x24
0018fde4 76fa5dae ffffffff 76fcb7ef 00000000 ntdll!__RtlUserThreadStart+0x2f
0018fdf4 00000000 7398dc60 002f1000 00000000 ntdll!_RtlUserThreadStart+0x1b


THREAD_SHA1_HASH_MOD_FUNC:  c3facf07e014be2a527afdc1a33e16566c6c8525

THREAD_SHA1_HASH_MOD_FUNC_OFFSET:  f7cb20a8ded02eb9a7e2bab762031456ddf7435d

THREAD_SHA1_HASH_MOD:  5e97547c8623c666632e914fb69095bf0598184b

FAULT_INSTR_CODE:  88c10ff0

SYMBOL_STACK_INDEX:  0

SYMBOL_NAME:  clr!TableFreeSingleHandleToCache+3f

FOLLOWUP_NAME:  MachineOwner

MODULE_NAME: clr

IMAGE_NAME:  clr.dll

DEBUG_FLR_IMAGE_TIMESTAMP:  56e1f25a

STACK_COMMAND:  ~0s ; kb

BUCKET_ID:  INVALID_POINTER_WRITE_AVRF_clr!TableFreeSingleHandleToCache+3f

PRIMARY_PROBLEM_CLASS:  INVALID_POINTER_WRITE_AVRF_clr!TableFreeSingleHandleToCache+3f

BUCKET_ID_OFFSET:  3f

BUCKET_ID_MODULE_STR:  clr

BUCKET_ID_MODTIMEDATESTAMP:  56e1f25a

BUCKET_ID_MODCHECKSUM:  6b6503

BUCKET_ID_MODVER_STR:  4.6.1078.0

BUCKET_ID_PREFIX_STR:  INVALID_POINTER_WRITE_AVRF_

FAILURE_PROBLEM_CLASS:  INVALID_POINTER_WRITE_AVRF

FAILURE_EXCEPTION_CODE:  c0000005

FAILURE_IMAGE_NAME:  clr.dll

FAILURE_FUNCTION_NAME:  TableFreeSingleHandleToCache

BUCKET_ID_FUNCTION_STR:  TableFreeSingleHandleToCache

FAILURE_SYMBOL_NAME:  clr.dll!TableFreeSingleHandleToCache

FAILURE_BUCKET_ID:  INVALID_POINTER_WRITE_AVRF_c0000005_clr.dll!TableFreeSingleHandleToCache

WATSON_STAGEONE_URL:  http://watson.microsoft.com/StageOne/GCHandleExecutionEngineExceptionRepro.exe/1.0.0.0/574426eb/clr.dll/4.6.1078.0/56e1f25a/c0000005/000d883b.htm?Retriage=1

TARGET_TIME:  2016-05-24T10:07:01.000Z

OSBUILD:  10586

OSSERVICEPACK:  0

SERVICEPACK_NUMBER: 0

OS_REVISION: 0

OSPLATFORM_TYPE:  x86

OSNAME:  Windows 10

OSEDITION:  Windows 10 WinNt SingleUserTS

USER_LCID:  0

OSBUILD_TIMESTAMP:  2015-10-30 04:46:21

BUILDDATESTAMP_STR:  160223-1728

BUILDLAB_STR:  th2_release_sec

BUILDOSVER_STR:  10.0.10586.162

ANALYSIS_SESSION_ELAPSED_TIME: 18f5

ANALYSIS_SOURCE:  UM

FAILURE_ID_HASH_STRING:  um:invalid_pointer_write_avrf_c0000005_clr.dll!tablefreesinglehandletocache

FAILURE_ID_HASH:  {d076268e-4327-cacb-c834-903fdd106684}

Followup:     MachineOwner
---------
```
