
Delete ApprovalUI
DBCC CHECKIDENT('[ApprovalUI]', RESEED, 0);
INSERT INTO ApprovalUI(
    ModuleORSubModule,
    [Level],
    ParentId,
    Checked,
    IsActive,
    IsDeleted,
    CreatedOn,
    LastModified,
    SerialNO

)
VALUES
    (
        'Urban',
        1,
        0,
        0, --- checked
1,
        0,
        '2022-11-10',
        '2022-11-10'
        , 1
    ),
    (
        'Town Planning',
        2,
        1,
        0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 2
    )
,
(
    'Sales',
    2,
    1,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 3
    )
, (
    'Operations',
    2,
    1,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 4
    )
, (
    'Billing',
    2,
    1,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 5
    )
, (
    'Litigations',
    2,
    1,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 6
    )
, (
    'Global Forms',
    2,
    1,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 7
    )
,
(
    'Demarcation Request',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 8
    )
,
(
    'Clearance Form',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 9
    )
,
(
    'Map Approval',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 10
    ),
(
    'Stock Creation',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 11
    ),
(
    'Possession Announcement',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 12
    ),
(
    'Re-design Request Form',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 13
    ),
(
    'Demarcation Form',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 14
    ),
(
    'Construction Security',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 15
    ),
(
    'Construction Monitoring form',
    3,
    2,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 16
 ),
(
    'Lead Generation Form',
    3,
    3,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 17
    )
,
(
    'PreSale',
    3,
    3,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 18
    )
,
(
    'Booking Form',
    3,
    3,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 19
    )
,
(
    'Member Profile Form',
    3,
    3,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 20
    )
,
(
    'Dealer Profile Form',
    3,
    3,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 21
    )
,
(
    'NDC Request For Member',
    3,
    4,
    0, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 22
    )
,
(
    'NDC1',
    3,
    4,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 23
    )
,
(
    'Transfer',
    3,
    4,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 24
 ),
(
    'NDC Request For Dealer',
    3,
    4,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 25
    ),
(
    'Payment Plan Setup',
    3,
    3,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 26
    ),
(
    'Demand Note',
    3,
    3,
    1, --- checked
1,
    0,
    '2022-11-10',
    '2022-11-10'
    , 27
    );

--Dummy PMSUser Not Mandantory for database only for testing central park

Delete PMSUser
DBCC CHECKIDENT('[PMSUser]', RESEED, 0);  

INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('1420', '36502-5506637-5', 'Hafiz Zain Ul Abidin', 'Zulfiqar Ali', 'Junior Architect', 'Architect', '5/18/93', 'G', '6/6/22', '800371001', 'Urban Developers', 'Administration (Developers)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('1220', '35201-4694418-7', 'Saad  Mehmood', 'Mehmood Khan', 'Drafts Man', 'Architect', '2/10/01', 'G', '12/24/21', '762693001', 'Urban Developers', 'Administration (Developers)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('1393', '38301-5214952-7', 'Muhammad Juniad', 'Muhammad Ramzan', 'Drafts Man', 'Architect', '1/12/99', 'G', '6/10/22', '795655001', 'Urban Developers', 'Site Staff (Developers)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('1187', '34603-4465417-3', 'Sarmad  Anees', 'Muhammad Anees', 'Junior Architect', 'Architect', '1/1/94', 'G', '12/6/21', '749014001', 'Urban Developers', 'Site Staff (Developers)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('1167', '81302-9521943-1', 'Adeel  Ahmad', 'Pervaiz Akhtar', 'Drafts Man', 'Architect', '12/20/94', 'G', '9/20/21', '776309001', 'Urban Developers', 'Site Staff (Developers)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('395', '36501-4476133-9', 'Muhammad Zohaib', 'Muhammad Hanif', 'Senior Draftsman', 'Town Planning', '12/5/93', 'G', '3/7/16', '329003001', 'Urban Developers', 'Site Staff (Services)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('283', '35202-1967197-3', 'Hassam Ali', 'Syed Shahid Hussain', 'Additional Chief Town Planner', 'Town Planning', '12/8/87', 'G', '3/24/15', '266883001', 'Urban Developers', 'Site Staff (Services)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('170', '33203-1463937-7', 'Khalid Mahmood', 'Muhammad Manzoor', 'Data Entry Operator', 'Town Planning', '1/1/70', 'G', '3/13/13', '254946001', 'Urban Developers', 'Administration (Developers)');
INSERT INTO PMSUser(EMP_CODE, NIC_NO, EMP_FULL_NAME, EMP_FATHER_NAM, DESIG_DESC, DEPARTMENT_DESC, EMP_BIRTH_DATE, SHIFT_DESC, JOINING_DATE, EMP_BANK_ACC_NO, PAY_ORG_DESC, PAY_CC_DESC) VALUES('1097', '32302-8974548-7', 'Muhammad  Jahanzaib', 'Allah Yar (Late)', 'Town Planner', 'Town Planning', '5/20/95', 'G', '6/1/21', '739265001', 'Urban Developers', 'Site Staff (Developers)');


--Permission Forms


Delete UserPermissionMapping
DBCC CHECKIDENT('[PermissionForms]', RESEED, 1);
Delete PermissionForms
DBCC CHECKIDENT('[PermissionForms]', RESEED, 1);
INSERT INTO PermissionForms(Name, Title, IsActive, IsDeleted, CreatedOn, CreatedBy, LastModified, ModifiedBy)
VALUES
    ('Town Planning Setup', 'Town Planning Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Stock Creation', 'Stock Creation', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Property Binding', 'Property Binding', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Possession Announcement', 'Possession Announcement', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Demarcation Request Form', 'Demarcation Request Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Clearance Form', 'Clearance Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Map Approval', 'Map Approval', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Demarcation Form', 'Demarcation Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Re-Design Request', 'Re-Design Request', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Construction Security Form', 'Construction Security Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Construction Monitoring Form', 'Construction Monitoring Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Sales Setup', 'Sales Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Lead Generation Form', 'Lead Generation Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Payment Plan Setup', 'Payment Plan Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Pre-Sale Approval Form', 'Pre-Sale Approval Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Booking Form', 'Booking Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Advance Applictaion On Plot', 'Advance Applictaion On Plot', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Operations Setup', 'Operations Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Dealer Renewal Form', 'Dealer Renewal Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Member Request For NDC', 'Member Request For NDC', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Dealer Request For NDC', 'Dealer Request For NDC', 1, 0, GETDATE(), null, GETDATE(), null),
    ('NDC-1', 'NDC-1', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Transfer Form', 'Transfer Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Surrender Form', 'Surrender Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Re-Surrender Form', 'Re-Surrender Form', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Promotions Setup', 'Promotions Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Promotions', 'Promotions', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Banners', 'Banners', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Deals Setup', 'Deals Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Deals', 'Deals', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Receipt Setup', 'Receipt Setup', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Receipt', 'Receipt', 1, 0, GETDATE(), null, GETDATE(), null),
    ('Billing Setup', 'Billing Setup', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Meter Type', 'Meter Type', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Meter Status', 'Meter Status', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Reading Officer', 'Reading Officer', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Meter Installation', 'Meter Installation', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Meter Reading', 'Meter Reading', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Meter Bill Generation', 'Meter Bill Generation', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Fixed Charges Bill Generation', 'Fixed Charges Bill Generation', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Individual Bill Generation', 'Individual Bill Generation', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Monthly Bill Generation', 'Monthly Bill Generation', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Litigation Setup', 'Litigation Setup', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Case Category', 'Case Category', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Case Type', 'Case Type', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Lawyer Data', 'Lawyer Data', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Case Profile', 'Case Profile', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('DemandNote Setup', 'DemandNote Setup', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Demand Note', 'Demand Note', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Demand Note HOD Action', 'Demand Note HOD Action', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Demand Note Custodian Action', 'Administration', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Administration Setup', 'Administration Setup', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Setup Forms Settings', 'Administration Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Town Planning', 'Setup Forms', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Phase Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Real Estate Type Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Project Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Block Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Category Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('UOM Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Floor Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Property Type Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Property Nature Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Features Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Finishes Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Prefix Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Postfix Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Construction Stage Definition', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Stock Creation Setup', 'Town Planning', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Sales Forms Settings', 'Setup Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Social Status Definition', 'Sales', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Lead Generation Form', 'Sales', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Member Registration', 'Sales', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Payment Plan Type', 'Sales', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Deal Forms Settings', 'Sales Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Dealer Category', 'Sales', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Operations Forms Settings', 'Operations Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Dealer Registration', 'Operations', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Billing Forms Settings', 'Billing Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Property Billing Setup', 'Billing', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Grace Period Setup', 'Permission Form for setting up grace periods', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Sale Tax Setup', 'Permission Form for setting up sales taxes', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('WithHolding Tax Setup', 'Permission Form for setting up withholding taxes', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Litigation Forms Settings', 'Litigation Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Forum', 'Permission Form for managing litigation forums', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Global Setup Forms Settings', 'Global Setup Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Charges Group', 'Permission Form for managing charges groups', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Charges Type', 'Permission Form for managing charges types', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Charges Setup', 'Permission Form for setting up charges', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Charge Group Forms Setup', 'Permission Form for managing charge group forms', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Violation Group', 'Permission Form for managing violation groups', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Violation Type', 'Permission Form for managing violation types', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Global Master Data Forms Settings', 'Global Master Data Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Property List', 'Permission Form for managing property lists', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Registration No. Profile', 'Permission Form for managing registration number profiles', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Property Profile', 'Property Profile', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Dealer Profile', 'Dealer Profile', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Member Profile', 'Member Profile', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('SAP DataBase Integration Forms Settings', 'SAP DataBase Integration Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Operations', 'Operations', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Billing', 'Billing', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('GL Determination', 'GL Determination', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Approvals Setup Forms Settings', 'Approvals Setup Forms Settings', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Role Permisison', 'Role Permisison', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('User Permisison', 'User Permisison', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Approval Tree', 'Approval Tree', 1, 0, GETDATE(), NULL, GETDATE(), NULL),
    ('Approval Setup', 'Approval Setup', 1, 0, GETDATE(), NULL, GETDATE(), NULL)