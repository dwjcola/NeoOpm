addressables 说明:
		Editor:
             当前使用版本:1.10.0 
			 目录结构:JJAddressableEditor为编辑器根目录
					Editor/Configs/AddressablesInfoConfig.xlsx 为打包配置文件 
						  sheet1：
						  GroupName设置组名称 
						  Path资源相对路径 
						  labelNames资源标签 
						  Update Restriction
						  Can Change Post Release    資源全量更新
						  Cannot Change Post Release 資源增量更新
						  isLocal Group下bundle路径 决定是随包进游戏 或 远程下载
						  BundleMode 对应Group下entry的打包方式
						  sheet2: 
						  开发模式和生产模式配置 资源更新下载路径
					ExcelDLLScripts	：解析excel库文件
					
					Scripts：编辑器脚本
							JJAddressableBuildEditor：打包 打patch用的脚本
							JJAnalyzeRuleEditor：自定义文件分析脚本 示例 可按需求扩展。
		
		Runtime:
			 Main.cs
			 脚本中CheckUpdates()方法 实现模拟 常规assetbundles启动检查更新方法.（条件JJAddressableBuildEditor.cs中AutoSetGroupLabel方法中的settings.DisableCatalogUpdateOnStartup = true ）
			 
				
