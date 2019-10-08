dotnet ?= dotnet
mode ?= debug
autoproj ?= autoproj

branch := $(notdir $(shell git symbolic-ref HEAD))

ifeq ($(OS),Windows_NT)
	/ := \\
	os := win
else
	/ := /
	os := default
endif

.PHONY: debug release publish autoproj publish exam test install_local_cover
debug:
	@echo "building(debug)..."
	@$(dotnet) build -nologo -c Debug GeminiLab.Core2.sln

release:
	@echo "building(release)..."
	@$(dotnet) build -nologo -c Release GeminiLab.Core2.sln

publish: autoproj
	@echo "building(publish)..."
	@$(dotnet) build -nologo -c Publish GeminiLab.Core2.sln

full_release: autoproj release
	@echo "done"

autoproj:
	@-$(autoproj)

exam:
	@$(dotnet) run -p Exam$(/)Exam.csproj

test:
	@$(dotnet) test -nologo -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:Exclude=[xunit.*]* -v=normal --no-build XUnitTester$(/)XUnitTester.csproj

local_cover: all test
	reportgenerator -reports:.$(/)XUnitTester$(/)coverage.opencover.xml -targetdir:report.ignore
