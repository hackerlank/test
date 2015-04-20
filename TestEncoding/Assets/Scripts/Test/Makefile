#$Id$

#test
proto-test:
	echo $(BUILD_DIR_ROOT)
	@echo $(PROTO_SRC_PATH)
	@echo $(PROTO_DST_PATH)
	@echo "开始移除文件"
	@echo "开始生成文件"
	@dir ..\..\..
	
	protogen -i:message.proto -o:message.cs
	@echo protogen -i:E:\work\Rose2\Src\Client\branches\Beige\message.proto -o:E:\work\Rose2\Src\Client\branches\message.cs
	@echo protogen -i:E:\work\Rose2\Src\RoseComm\Cmd\branche_Rose\message.proto -o:E:\work\Rose2\Src\Client\branches\Beige\Assets\Scripts\ProtoBuf\message.cs
	@echo "结束" RoseComm\branche_Rose
#makefile 放在Rose目录下，打开cmd,执行命令： make proto
proto:
	protogen -i:Src\RoseComm\Cmd\branche_Rose\message.proto -o:Src\Client\branches\Beige\Assets\Scripts\ProtoBuf\message.cs
	@echo "结束" 
#makefile 放在Src目录下	
proto2:
	protogen -i:RoseComm\Cmd\branche_Rose\message.proto -o:Client\branches\Beige\Assets\Scripts\ProtoBuf\message.cs
	@echo "结束" 
#makefile 放在Src或上级目录下	
proto3:
	protogen -i:E:\work\Rose2\Src\RoseComm\Cmd\branche_Rose\message.proto -o:E:\work\Rose2\Src\Client\branches\Beige\Assets\Scripts\ProtoBuf\message.cs
	@echo "结束"
	

	
#makefile 放在Client目录,这个有问题，不能用
proto4:
	protogen -i:..\RoseComm\Cmd\branche_Rose\message.proto -o:branches\Beige\Assets\Scripts\ProtoBuf\message.cs
	@echo "结束"