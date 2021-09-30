test:
	dotnet test --verbosity normal /p:CollectCoverage=true

run:
	dotnet run --project texo.api
	
run-docker:
	docker-compose up --build