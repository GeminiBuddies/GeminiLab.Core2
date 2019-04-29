dotnet ?= dotnet
mode ?= debug
autoproj ?= autoproj

ifeq ($(OS),Windows_NT)
	/ := \
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

.PHONY: all autoproj publish run_test
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

run_test:
	@$(dotnet) run -p TestConsole$(/)TestConsole.csproj
