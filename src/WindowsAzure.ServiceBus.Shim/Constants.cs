using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.ServiceBus.Messaging;

internal static class Constants
{
    public const string Namespace = "http://schemas.microsoft.com/netservices/2011/06/servicebus";

    public const bool DefaultEnableDeadLetteringOnMessageExpiration = false;

    public const bool DefaultEnableDeadLetteringOnFilterEvaluationExceptions = true;

    public const bool DefaultEnableBatchedOperations = true;

    public const bool DefaultIsAnonymousAccessible = false;

    public const bool DefaultIsClientAffine = false;

    public const bool DefaultIsDurable = true;

    public const bool DefaultIsShared = true;

    public const bool DefaultEnablePublicationIdentification = false;

    public static readonly TimeSpan DefaultOperationTimeout = TimeSpan.FromMinutes(1.0);

    public static readonly TimeSpan MaxOperationTimeout = TimeSpan.FromDays(1.0);

    public static readonly TimeSpan TokenRequestOperationTimeout = TimeSpan.FromMinutes(3.0);

    public const string DefaultOperationTimeoutString = "0.00:01:00.00";

    public static readonly int ServicePointMaxIdleTimeMilliSeconds = 50000;

    public static readonly TimeSpan DefaultBatchFlushInterval = TimeSpan.FromMilliseconds(20.0);

    public static readonly int DefaultBatchFlushIntervalInMilliseconds = 20;

    public static readonly int DefaultBatchSchedulerLevel1Threshold = 10;

    public const string DefaultBatchFlushIntervalString = "0.00:00:00.20";

    public static readonly double DefaultUsedSpaceAlertPercentage = 70.0;

    public static readonly TimeSpan DefaultLockDuration = TimeSpan.FromSeconds(60.0);

    public static readonly TimeSpan DefaultDuplicateDetectionHistoryExpiryDuration = TimeSpan.FromMinutes(10.0);

    public static readonly TimeSpan DefaultAllowedTimeToLive = TimeSpan.MaxValue;

    public static readonly TimeSpan MaximumAllowedTimeToLive = TimeSpan.MaxValue;

    public static readonly TimeSpan MaximumAllowedTimeToLiveForBasicSku = TimeSpan.FromDays(14.0);

    public static readonly TimeSpan DefaultAllowedTimeToLiveForBasicSku = TimeSpan.FromDays(14.0);

    public static readonly TimeSpan PartitionedEntityMaximumAllowedTimeToLive = TimeSpan.MaxValue;

    public static readonly TimeSpan PartitionedEntityDefaultAllowedTimeToLive = PartitionedEntityMaximumAllowedTimeToLive;

    public static readonly TimeSpan MinimumAllowedTimeToLive = TimeSpan.FromSeconds(1.0);

    public static readonly TimeSpan MinimumLockDuration = TimeSpan.FromSeconds(5.0);

    public static readonly TimeSpan MaximumLockDuration = TimeSpan.FromMinutes(5.0);

    public static readonly TimeSpan MaximumRenewBufferDuration = TimeSpan.FromSeconds(10.0);

    public static readonly TimeSpan DebuggingLockDuration = TimeSpan.FromDays(1.0);

    public static readonly TimeSpan MaximumAllowedIdleTimeoutForAutoDelete = TimeSpan.MaxValue;

    public static readonly int MaximumTagSize = 120;

    public const int MinimumSizeForCompression = 200;

    public const long DefaultMessageRetentionInDays = 7L;

    public const long DefaultMessageRetentionInDaysForBasicSku = 1L;

    public const long MaxMessageRetentionInDaysForBasicSku = 1L;

    public const int DefaultMinMessageRetentionInDays = 1;

    public const int DefaultMaxMessageRetentionInDays = 7;

    public const bool DefaultEnableCheckpoint = false;

    public static CursorType DefaultCursorType = CursorType.Server;

    public static readonly TimeSpan MaximumDuplicateDetectionHistoryTimeWindow = TimeSpan.FromDays(7.0);

    public static readonly TimeSpan MinimumDuplicateDetectionHistoryTimeWindow = TimeSpan.FromSeconds(20.0);

    public static readonly TimeSpan DefaultRetryDelay = TimeSpan.FromSeconds(10.0);

    public static readonly int DefaultRetryLimit = 3;

    public static readonly int DefaultSqlFlushThreshold = 4500;

    public static readonly int FlushBatchThreshold = 100;

    public static readonly int MaximumRequestSchedulerQueueDepth = 15000;

    public static readonly int MaximumBatchSchedulerQueueDepth = 100000;

    public static readonly int MaximumEntityNameLength = 400;

    public static readonly int MaximumMessageHeaderPropertySize = 65535;

    public static readonly string ContainerShortName = ".";

    public static readonly int DefaultClientPumpPrefetchCount = 10;

    public static readonly int DefaultPrefetchCount = 0;

    public static readonly int DefaultEventHubPrefetchCount = 300;

    public static readonly int EventHubMinimumPrefetchCount = 10;

    public static readonly long EventHubMinimumPrefetchSizeInBytes = 266240L;

    public static readonly long EventHubMaximumPrefetchSizeInBytes = long.MaxValue;

    public static readonly int DefaultMessageSessionPrefetchCount = 0;

    public static readonly int DefaultMaxDeliveryCount = 10;

    public static readonly int MinAllowedMaxDeliveryCount = 1;

    public static readonly int MaxAllowedMaxDeliveryCount = int.MaxValue;

    public static readonly long DefaultLastPeekedSequenceNumber = 0L;

    public static readonly TimeSpan DefaultRegistrationTtl = TimeSpan.MaxValue;

    public static readonly TimeSpan MinimumRegistrationTtl = TimeSpan.FromDays(1.0);

    public const string NotificationHub = "NotificationHub";

    public const string NotificationHubs = "NotificationHubs";

    public const string Queue = "Queue";

    public const string Queues = "Queues";

    public const string QueuesInLowerCase = "queues";

    public const string Topic = "Topic";

    public const string Topics = "Topics";

    public const string TopicsInLowerCase = "topics";

    public const string EventHub = "EventHub";

    public const string EventHubs = "EventHubs";

    public const string SchemaGroup = "SchemaGroup";

    public const string SchemaGroups = "SchemaGroups";

    public const string SchemaGroupNames = "SchemaGroupNames";

    public const string Schema = "Schema";

    public const string SchemaById = "SchemaById";

    public const string SchemaNames = "SchemaNames";

    public const string SchemaVersions = "SchemaVersions";

    public const string Relay = "Relay";

    public const string Relays = "Relays";

    public const string HybridConnection = "HybridConnection";

    public const string HybridConnections = "HybridConnections";

    public const string RevokedPublisher = "RevokedPublisher";

    public const string RevokedPublishers = "RevokedPublishers";

    public const string ConsumerGroup = "ConsumerGroup";

    public const string ConsumerGroups = "ConsumerGroups";

    public const string Subscription = "Subscription";

    public const string Subscriptions = "Subscriptions";

    public const string SubscriptionsInLowerCase = "subscriptions";

    public const string Partition = "Partition";

    public const string Partitions = "Partitions";

    public const string Publishers = "Publishers";

    public const string Rule = "Rule";

    public const string Rules = "Rules";

    public const string All = "All";

    public const string NamespaceInfo = "NamespaceInfo";

    public static readonly string Windows = "windows";

    public static readonly string IssuedToken = "issuedToken";

    public static readonly string Anonymous = "anonymous";

    public const string ResourceGroupsInLowerCase = "resourcegroups";

    public const string ClustersInLowerCase = "clusters";

    public const string AuthClaimType = "net.windows.servicebus.action";

    public const string ManageClaim = "Manage";

    public const string SendClaim = "Send";

    public const string ListenClaim = "Listen";

    public const string SchemaWriteClaim = "SchemaWrite";

    public const string SchemaReadClaim = "SchemaRead";

    public const string SchemaDeleteClaim = "SchemaDelete";

    public const string SchemaGroupWriteClaim = "SchemaGroupWrite";

    public const string SchemaGroupReadClaim = "SchemaGroupRead";

    public const string SchemaGroupDeleteClaim = "SchemaGroupDelete";

    public const string EntityWriteClaim = "EntityWrite";

    public const string EntityReadClaim = "EntityRead";

    public const string EntityDeleteClaim = "EntityDelete";

    public const string ChildEntityWriteClaim = "ChildEntityWrite";

    public const string ChildEntityReadClaim = "ChildEntityRead";

    public const string ChildEntityDeleteClaim = "ChildEntityDelete";

    public const string AuthRuleWriteClaim = "AuthRuleWrite";

    public const string AuthRuleReadClaim = "AuthRuleRead";

    public const string AuthRuleDeleteClaim = "AuthRuleDelete";

    public const string SubscriptionRuleWriteClaim = "SubscriptionRuleWrite";

    public const string SubscriptionRuleReadClaim = "SubscriptionRuleRead";

    public const string SubscriptionRuleDeleteClaim = "SubscriptionRuleDelete";

    public const string ListKeysClaim = "ListKeys";

    public static List<string> SupportedClaims = new()
    {
            "Manage",
            "Send",
            "Listen"
        };

    public const int SupportedClaimsCount = 3;

    public const string ClaimSeparator = ",";

    public const char ClaimSeparatorChar = ',';

    public const string SystemKeyName = "$system";

    public const string PathDelimiter = "/";

    public const char PathDelimiterAsChar = '/';

    public const string SubQueuePrefix = "$";

    public const string EntityDelimiter = "|";

    public const string EmptyEntityDelimiter = "||";

    public const string ColonDelimiter = ":";

    public const string PartDelimiter = ":";

    public const string DeadLetterQueueSuffix = "DeadLetterQueue";

    public const string DeadLetterQueueSuffixInLowerCase = "deadletterqueue";

    public const string DeadLetterQueueName = "$DeadLetterQueue";

    public const string DeadLetterQueueNameInLowerCase = "$deadletterqueue";

    public static List<string> SupportedSubQueueNames = new()
    {
            "$DeadLetterQueue"
        };

    public const string RuleNameHeader = "RuleName";

    public const string DeadLetterReasonHeader = "DeadLetterReason";

    public const string DeadLetterSource = "DeadLetterSource";

    public const string DeadLetterErrorDescriptionHeader = "DeadLetterErrorDescription";

    public const string TransferQueueSuffix = "Transfer";

    public const string TransferQueueName = "$Transfer";

    public const string TTLExpiredExceptionType = "TTLExpiredException";

    public const string MaxDeliveryCountExceededExceptionType = "MaxDeliveryCountExceeded";

    public const string HeaderSizeExceededExceptionType = "HeaderSizeExceeded";

    public const string IsAnonymousAccessibleHeader = "X-MS-ISANONYMOUSACCESSIBLE";

    public const string ETag = "ETag";

    public const string DateTime2Format = "yyyy-MM-ddTHH:mm:ss.fff";

    public const string ServiceBusSupplementartyAuthorizationHeaderName = "ServiceBusSupplementaryAuthorization";

    public const string ServiceBusDlqSupplementaryAuthorizationHeaderName = "ServiceBusDlqSupplementaryAuthorization";

    public const string ServiceBusOriginalViaProperty = "ServiceBusOriginalVia";

    public const string ServiceBusViaAliasProperty = "ServiceBusViaAliasProperty";

    public const string ServiceBusNamespaceFromUriProperty = "ServiceBusNamespaceFromUriProperty";

    public const string ParentLinkIdProperty = "ParentLinkId";

    public const string DisableOperationalLogConfigName = "disableOperationalLog";

    public const string TransactionWorkUnit = "TxnWorkUnit";

    public const string WorkUnitInfo = "WorkUnitInfo";

    public const string Identifier = "Identifier";

    public const string SequenceNumber = "SequenceNumber";

    public const string ContinuationTokenHeaderName = "x-ms-continuationtoken";

    public const string ContinuationTokenQueryName = "continuationtoken";

    public const string AuthorizationHeaderName = "Microsoft.Cloud.ServiceBus.HttpAutorizationhHeaders";

    public const string HstsHeaderName = "Strict-Transport-Security";

    public const string RetryPolicyHeaderName = "x-ms-retrypolicy";

    public const string NodeScheme = "node";

    public static readonly Type ConstantsType = typeof(Constants);

    public static readonly Type MessageType = typeof(BrokeredMessage);

    public static readonly Type GuidType = typeof(Guid);

    public static readonly Type ObjectType = typeof(object);

    public static readonly MethodInfo? NewGuid = GuidType.GetMethod("NewGuid", BindingFlags.Static | BindingFlags.Public);

    public const string SystemScope = "sys";

    public const string UserScope = "user";

    public const int DefaultCompatibilityLevel = 20;

    public const string DefaultEntityName = "$Default";

    public const string DurableSubscriberTag = "D";

    public const string NonDurableSubscriberTag = "ND";

    public const int MaximumRuleActionStatements = 32;

    public const int MaximumSqlFilterStatementLength = 1024;

    public const int MaximumSqlRuleActionStatementLength = 1024;

    public const int MaximumLambdaExpressionNodeCount = 1024;

    public const int MaximumLambdaExpressionTreeDepth = 32;

    public const int MaxMessageIdLength = 128;

    public const int MaxSessionIdLength = 128;

    public const int MaxPartitionKeyLength = 128;

    public const int MaxDestinationLength = 128;

    public const int MaxJobIdLength = 128;

    public const int QueueNameMaximumLength = 260;

    public const int TopicNameMaximumLength = 260;

    public const int EventHubNameMaximumLength = 260;

    public const int NotificationHubNameMaximumLength = 260;

    public const int ConsumerGroupNameMaximumLength = 50;

    public const int SubscriptionNameMaximumLength = 50;

    public const int SubscriptionClientIdMaximumLength = 50;

    public const int SubscriptionNameWithClientIdMaximumLength = 104;

    public const int PartitionNameMaximumLength = 50;

    public const int RuleNameMaximumLength = 50;

    public const int SchemaGroupNameMaximumLength = 260;

    public const int SchemaNameMaximumLength = 100;

    public const string BrokerInvalidOperationPrefix = "BR0012";

    public const string InternalServiceFault = "InternalServiceFault";

    public const string ConnectionFailedFault = "ConnectionFailedFault";

    public const string EndpointNotFoundFault = "EndpointNotFoundFault";

    public const string AuthorizationFailedFault = "AuthorizationFailedFault";

    public const string NoTransportSecurityFault = "NoTransportSecurityFault";

    public const string QuotaExceededFault = "QuotaExceededFault";

    public const string PartitionNotOwnedFault = "PartitionNotOwnedException";

    public const string UndeterminableExceptionType = "UndeterminableExceptionType";

    public const string InvalidOperationFault = "InvalidOperationFault";

    public const string SessionLockLostFault = "SessionLockLostFault";

    public const string TimeoutFault = "TimeoutFault";

    public const string ArgumentFault = "ArgumentFault";

    public const string MessagingEntityDisabledFault = "MessagingEntityDisabledFault";

    public const string ServerBusyFault = "ServerBusyFault";

    public const string MessagingEntityNotFoundFault = "MessagingEntityNotFoundFault";

    public const int MaxSizeInMegabytes = 1024;

    public const long StandardDefaultMaxMessageSizeInKilobytes = 256L;

    public const long PremiumDefaultMaxMessageSizeInKilobytes = 1024L;

    public const long PremiumLargeMessageMaxMessageSizeInKilobytes = 102400L;

    public const int DefaultConnectionTimeoutInSeconds = 30;

    public const int MaximumUserMetadataLength = 1024;

    public const int MaxNotificationHubPathLength = 290;

    public static readonly TimeSpan AutoDeleteOnIdleDefaultValue = DefaultAllowedTimeToLive;

    public static readonly TimeSpan DefaultRetryMinBackoff = TimeSpan.FromSeconds(0.0);

    public static readonly TimeSpan DefaultRetryMaxBackoff = TimeSpan.FromSeconds(30.0);

    public static readonly TimeSpan DefaultRetryDeltaBackoff = TimeSpan.FromSeconds(3.0);

    public static readonly TimeSpan DefaultRetryTerminationBuffer = TimeSpan.FromSeconds(5.0);

    public const int DefaultMaxRetryCount = 10;

    public const string HttpErrorSubCodeFormatString = "SubCode={0}";

    public const long DefaultStartingCheckpoint = 0L;

    public const long MaxEventHubConnectionFailureBeforeReset = 10L;

    public const string MGLogicalPartitionCountConfigName = "mglogicalpartitioncount";

    public const string BacklogQueueNameBaseFormatString = "{0}/x-servicebus-transfer/";

    public const string BacklogQueueNameFormatString = "{0}/x-servicebus-transfer/{1}";

    public const string BacklogQueueNameQueryString = "startswith(path, '{0}/x-servicebus-transfer/') eq true";

    public const string BacklogScheduledEnqueueTimeUtcProperty = "x-ms-scheduledenqueuetimeutc";

    public const string BacklogSessionIdProperty = "x-ms-sessionid";

    public const string BacklogTimeToLiveProperty = "x-ms-timetolive";

    public const string BacklogPathProperty = "x-ms-path";

    public const int BacklogBatchReceiveSize = 100;

    public const string BacklogPingMessageContentType = "application/vnd.ms-servicebus-ping";

    public const int BacklogQueueSize = 5120;

    public static readonly TimeSpan DefaultPrimaryFailoverInterval = TimeSpan.FromMinutes(1.0);

    public static readonly TimeSpan MinPrimaryFailoverInterval = TimeSpan.Zero;

    public static readonly TimeSpan MaxPrimaryFailoverInterval = TimeSpan.FromMinutes(60.0);

    public static readonly TimeSpan GetRuntimeEntityDescriptionTimeout = TimeSpan.FromHours(1.0);

    public static readonly TimeSpan GetRuntimeEntityDescriptionNonTransientSleepTimeout = TimeSpan.FromMinutes(10.0);

    public static readonly Uri AnonymousUri = new("http://www.w3.org/2005/08/addressing/anonymous");

    public static readonly TimeSpan ClientPumpRenewLockTimeout = TimeSpan.FromMinutes(5.0);

    public const string NullString = "(null)";

    public const string Batch = "-Batch";

    public const string Send = "Send";

    public const string TryReceive = "TryReceive";

    public const string SendBatch = "Send-Batch";

    public const string TryReceiveBatch = "TryReceive-Batch";

    public const string RenewLock = "RenewLock";

    public const string RenewLockBatch = "RenewLock-Batch";

    public const string RenewSessionLock = "RenewSessionLock";

    public const string Peek = "Peek";

    public const string PeekBatch = "Peek-Batch";

    public const string SetSessionState = "SetSessionState";

    public const string GetSessionState = "GetSessionState";

    public const string BadCommand = "BadCommand";

    public const string ScheduleMessage = "ScheduleMessage";

    public const string CancelScheduledMessage = "CancelScheduledMessage";

    public const string CancelScheduledMessageBatch = "CancelScheduledMessage-Batch";

    public const string AmqpRedirectProtocol = "AmqpRedirect";

    public const string AddRule = "AddRule";

    public const string DeleteRule = "DeleteRule";

    public const string EnumerateRules = "EnumerateRules";

    public const string UpdateDisposition = "UpdateDisposition";

    public const string GetMessageSessions = "GetMessageSessions";

    public const string ProductName = "product";

    public const string VersionName = "version";

    public const string PlatformName = "platform";

    public const string FrameworkName = "framework";

    public const string UserAgentName = "user-agent";

    public const string HttpLocationHeaderName = "Location";

    public const char HostFqdnToContainerIdSeparator = ':';

    public const long DefaultMaxSizeEventHub = 10995116277760L;

    public const int DefaultEventHubPartitionCount = 4;

    public const bool DefaultHubIsDisabled = false;

    //
    // Summary:
    //     This string is used by client to match the hostname of the endpoint to see if
    //     this is a IoT related endpoint, and if so we treat it special (mainly to assume
    //     client side redirect for these endpoint).
    public const string IoTDeviceUriSuffix = ".azure-devices.net";

    public const string IoTDeviceUriInternalSuffix = ".azure-devices-int.net";

    public const string IoTDeviceUriBlackForestSuffix = ".azure-devices.de";

    public const string IoTDeviceUriMooncakeSuffix = ".azure-devices.cn";

    public const string IoTDeviceUriFairfaxSuffix = ".azure-devices.us";

    public const string IoTAmqpSaslPlainKeyNameFormat = "{0}@sas.root.{1}";

    public static Func<string, byte[]> MessagingTokenProviderKeyEncoder = Encoding.UTF8.GetBytes;

    public static Func<string, byte[]> IoTTokenProviderKeyEncoder = Convert.FromBase64String;

    public const int MaxReceiverNameLength = 64;

    public const string EvenHubProviderNameSpace = "Microsoft Azure EventHub";

    public const string EvenHubAdminProviderNameSpace = "Microsoft Azure EventHub Admin";

    public const string ServiceBusProviderNameSpace = "Microsoft Azure ServiceBus";

    public const string ServiceBusServiceName = "ServiceBus";

    public const string EntityTypeSubscription = "Subscription";
}

[DataContract(Name = "CursorType", Namespace = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect")]
public enum CursorType
{
    //
    // Summary:
    //     The server cursor type.
    [EnumMember]
    Server,
    //
    // Summary:
    //     The client cursor type.
    [EnumMember]
    Client
}