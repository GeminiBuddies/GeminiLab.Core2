dotnet ?= dotnet
mode ?= debug
autoproj ?= autoproj

ifeq ($(OS),Windows_NT)
	/ := \\
	os := win
else
	/ := /
	os := default
endif

ifeq ($(mode), debug)
	mode_str := Debug
else ifeq ($(mode), release)
	mode_str := Release
else ifeq ($(mode), publish)
	mode_str := Publish
else
	mode_str := Error
endif

.PHONY: all autoproj publish exam test install_local_cover
all: autoproj
ifeq ($(mode_str), Error)
	@echo "unknown mode $(mode)."
	@exit 1
else
	@echo "building for $(mode)..."
	@$(dotnet) build -nologo -c $(mode_str) GeminiLab.Core2.sln
endif

autoproj:
	@-$(autoproj)

publish:
	@$(MAKE) --no-print-directory dotnet=$(dotnet) autoproj=$(autoproj) mode=publish

exam:
	@$(dotnet) run -p Exam$(/)Exam.csproj

test: autoproj
	@$(dotnet) test -nologo -p:CollectCoverage=true -p:CoverletOutputFormat=opencover -p:Exclude=[xunit.*]* XUnitTester$(/)XUnitTester.csproj

local_cover: test
	reportgenerator -reports:.$(/)XUnitTester$(/)coverage.opencover.xml -targetdir:report.ignore

install_local_cover:
	dotnet tool install -g dotnet-reportgenerator-globaltool
