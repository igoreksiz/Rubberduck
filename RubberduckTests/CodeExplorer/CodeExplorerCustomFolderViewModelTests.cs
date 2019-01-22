﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rubberduck.Navigation.CodeExplorer;
using Rubberduck.Navigation.Folders;
using Rubberduck.Parsing.Symbols;

namespace RubberduckTests.CodeExplorer
{
    [TestFixture]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class CodeExplorerCustomFolderViewModelTests
    {
        [Test]
        [Category("Code Explorer")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo"}, TestName = "Constructor_SetsFolderName_RootFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar" }, TestName = "Constructor_SetsFolderName_SubFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz" }, TestName = "Constructor_SetsFolderName_SubSubFolder")]
        public void Constructor_SetsFolderName(object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var folderPath = structure.First().Folder;
            var path = folderPath.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out _);
            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref declarations);

            foreach (var name in path)
            {
                Assert.AreEqual(name, folder.Name);
                folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>().FirstOrDefault();
            }
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo" }, TestName = "Constructor_SetsFullPath_RootFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar" }, TestName = "Constructor_SetsFullPath_SubFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz" }, TestName = "Constructor_SetsFullPath_SubSubFolder")]
        public void Constructor_SetsFullPath(object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var folderPath = structure.First().Folder;
            var path = folderPath.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out _);
            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref declarations);

            var depth = 1;
            foreach (var _ in path)
            {
                Assert.AreEqual(string.Join(FolderExtensions.FolderDelimiter.ToString(), path.Take(depth++)), folder.FullPath);
                folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>().FirstOrDefault();
            }
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo" }, TestName = "Constructor_PanelTitleIsFullPath_RootFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar" }, TestName = "Constructor_PanelTitleIsFullPath_SubFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz" }, TestName = "Constructor_PanelTitleIsFullPath_SubSubFolder")]
        public void Constructor_PanelTitleIsFullPath(object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var folderPath = structure.First().Folder;
            var path = folderPath.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out _);
            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref declarations);

            foreach (var _ in path)
            {
                Assert.AreEqual(folder.FullPath, folder.PanelTitle);
                folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>().FirstOrDefault();
            }
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo" }, TestName = "Constructor_PanelTitleIsFullPath_RootFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar" }, TestName = "Constructor_PanelTitleIsFullPath_SubFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz" }, TestName = "Constructor_PanelTitleIsFullPath_SubSubFolder")]
        public void Constructor_FolderAttributeIsCorrect(object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var folderPath = structure.First().Folder;
            var path = folderPath.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out _);
            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref declarations);

            foreach (var _ in path)
            {
                Assert.AreEqual($"'@Folder(\"{folder.FullPath}\")", folder.FolderAttribute);
                folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>().FirstOrDefault();
            }
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo" }, TestName = "Constructor_DescriptionIsFolderAttribute_RootFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar" }, TestName = "Constructor_DescriptionIsFolderAttribute_SubFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz" }, TestName = "Constructor_DescriptionIsFolderAttribute_SubSubFolder")]
        public void Constructor_DescriptionIsFolderAttribute(object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var folderPath = structure.First().Folder;
            var path = folderPath.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out _);
            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref declarations);

            foreach (var _ in path)
            {
                Assert.AreEqual(folder.FolderAttribute, folder.Description);
                folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>().FirstOrDefault();
            }
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo" }, TestName = "Constructor_SetsFolderDepth_RootFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar" }, TestName = "Constructor_SetsFolderDepth_SubFolder")]
        [TestCase(new object[] { CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz" }, TestName = "Constructor_SetsFolderDepth_SubSubFolder")]
        public void Constructor_SetsFolderDepth(object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var folderPath = structure.First().Folder;
            var path = folderPath.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out _);
            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref declarations);

            var depth = 1;
            foreach (var _ in path)
            {
                Assert.AreEqual(depth++, folder.FolderDepth);
                folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>().FirstOrDefault();
            }
        }

        [Test]
        [Category("Code Explorer")]
        public void FilteredIsFalseForAnyCharacter()
        {
            const string folderName = "Foo";
            const string testCharacters = "abcdefghijklmnopqrstuwxyz";

            var testFolder = (Name: CodeExplorerTestSetup.TestModuleName, Folder: folderName);
            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(new List<(string Name, string Folder)> { testFolder }, out _);

            var folder = new CodeExplorerCustomFolderViewModel(null, folderName, folderName, null, ref declarations);

            foreach (var character in testCharacters.ToCharArray().Select(letter => letter.ToString()))
            {
                folder.Filter = character;
                Assert.IsFalse(folder.Filtered);
            }
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(CodeExplorerSortOrder.Undefined)]
        [TestCase(CodeExplorerSortOrder.Name)]
        [TestCase(CodeExplorerSortOrder.CodeLine)]
        [TestCase(CodeExplorerSortOrder.DeclarationType)]
        [TestCase(CodeExplorerSortOrder.DeclarationTypeThenName)]
        [TestCase(CodeExplorerSortOrder.DeclarationTypeThenCodeLine)]
        public void SortComparerIsName(CodeExplorerSortOrder order)
        {
            const string folderName = "Foo";

            var testFolder = (Name: CodeExplorerTestSetup.TestModuleName, Folder: folderName);
            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(new List<(string Name, string Folder)> { testFolder }, out _);

            var folder = new CodeExplorerCustomFolderViewModel(null, folderName, folderName, null, ref declarations)
            {
                SortOrder = order
            };

            Assert.AreEqual(CodeExplorerItemComparer.Name.GetType(), folder.SortComparer.GetType());
        }

        [Test]
        [Category("Code Explorer")]
        public void ErrorStateCanNotBeSet()
        {
            const string folderName = "Foo";

            var testFolder = (Name: CodeExplorerTestSetup.TestModuleName, Folder: folderName);
            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(new List<(string Name, string Folder)> { testFolder }, out _);

            var folder = new CodeExplorerCustomFolderViewModel(null, folderName, folderName, null, ref declarations)
            {
                IsErrorState = true
            };

            Assert.IsFalse(folder.IsErrorState);
        }

        [Test]
        [Category("Code Explorer")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo.Modules", 
                  CodeExplorerTestSetup.TestClassName, "Foo.Classes", 
                  CodeExplorerTestSetup.TestDocumentName, "Foo.Docs", 
                  CodeExplorerTestSetup.TestUserFormName, "Foo.Forms")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo",
                  CodeExplorerTestSetup.TestClassName, "Foo.Bar",
                  CodeExplorerTestSetup.TestDocumentName, "Foo.Bar",
                  CodeExplorerTestSetup.TestUserFormName, "Foo.Bar")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo",
                  CodeExplorerTestSetup.TestClassName, "Foo.Bar",
                  CodeExplorerTestSetup.TestDocumentName, "Foo.Bar.Baz",
                  CodeExplorerTestSetup.TestUserFormName, "Foo.Bar")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo.Bar.Baz",
                  CodeExplorerTestSetup.TestClassName, "Foo.Bar.Baz",
                  CodeExplorerTestSetup.TestDocumentName, "Foo.Bar.Baz",
                  CodeExplorerTestSetup.TestUserFormName, "Foo.Bar.Baz")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo.Baz",
                  CodeExplorerTestSetup.TestClassName, "Foo.Baz",
                  CodeExplorerTestSetup.TestDocumentName, "Foo.Bar",
                  CodeExplorerTestSetup.TestUserFormName, "Foo.Bar")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo",
                  CodeExplorerTestSetup.TestClassName, "Foo.Bar.Baz",
                  CodeExplorerTestSetup.TestDocumentName, "Foo.Bar.Baz",
                  CodeExplorerTestSetup.TestUserFormName, "Foo.Foo.Foo")]
        [TestCase(CodeExplorerTestSetup.TestModuleName, "Foo Bar",
                  CodeExplorerTestSetup.TestClassName, "Foo Bar.Baz Baz",
                  CodeExplorerTestSetup.TestDocumentName, "Foo Bar.Baz",
                  CodeExplorerTestSetup.TestUserFormName, "Foo Bar.Foo Foo")]
        public void Constructor_CreatesCorrectSubFolderStructure(params object[] parameters)
        {
            var structure = ToFolderStructure(parameters.Cast<string>());
            var root = structure.First().Folder;
            var path = root.Split(FolderExtensions.FolderDelimiter);

            var declarations = CodeExplorerTestSetup.TestProjectWithFolderStructure(structure, out var projectDeclaration);
            var contents = CodeExplorerProjectViewModel.ExtractTrackedDeclarationsForProject(projectDeclaration, ref declarations);

            var folder = new CodeExplorerCustomFolderViewModel(null, path.First(), path.First(), null, ref contents);

            AssertFolderStructureIsCorrect(folder, structure);
        }

        private static void AssertFolderStructureIsCorrect(CodeExplorerCustomFolderViewModel underTest, List<(string Name, string Folder)> structure)
        {
            foreach (var (name, fullPath) in structure)
            {
                var folder = underTest;
                var path = fullPath.Split(FolderExtensions.FolderDelimiter);
                var depth = path.Length;

                for (var sub = 1; sub < depth; sub++)
                {
                    folder = folder.Children.OfType<CodeExplorerCustomFolderViewModel>()
                        .SingleOrDefault(subFolder =>
                            subFolder.FullPath.Equals(string.Join(FolderExtensions.FolderDelimiter.ToString(),
                                path.Take(folder.FolderDepth + 1))));
                }

                Assert.IsNotNull(folder);

                var components = folder.Children.OfType<CodeExplorerComponentViewModel>().ToList();
                var component = components.SingleOrDefault(subFolder => subFolder.Name.Equals(name));

                Assert.IsNotNull(component);

                var expected = structure.Where(item => item.Folder.Equals(fullPath)).Select(item => item.Name).OrderBy(_ => _);
                var actual = components.Select(item => item.Declaration.IdentifierName).OrderBy(_ => _);

                Assert.IsTrue(expected.SequenceEqual(actual));
            }
        }

        private static List<(string Name, string Folder)> ToFolderStructure(IEnumerable<string> structure)
        {
            var input = structure.ToArray();
            var output = new List<(string Name, string Folder)>();

            for (var module = 0; module < input.Length; module += 2)
            {
                output.Add((Name: input[module], Folder: input[module + 1]));
            }

            return output;
        }
    }
}
