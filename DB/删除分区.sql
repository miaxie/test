

----ɾ���ļ���
--alter database test remove filegroup  fileGroup01
---ɾ�������ļ����ļ���
--alter database test remove file  test01
--ɾ����������
--drop partition function [PF_ScoreHistory]
--ɾ����������
--drop PARTITION SCHEME [PS_ScoreHistory]

--�޸ı߽�ֵ
ALTER PARTITION SCHEME [PS_card] NEXT USED [fileGroup01]
alter partition function [PF_card]()  SPLIT RANGE ('2047-01-01')

ALTER PARTITION SCHEME [PS_scoreHistory] NEXT USED [fileGroup01]
alter partition function [PF_scoreHistory]()  SPLIT RANGE ('2021-01-01')


-- �鿴ָ�������е����ݼ�¼
select * from scoreHistory where $partition.[PF_ScoreHistory](createdate)=3
select * from sys.partition_schemes -- ��ѯ����
select * from sys.partition_range_values -- ��ѯ������Χ
select * from sys.partition_functions --��ѯ��������
