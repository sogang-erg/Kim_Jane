using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class Program
{
    static void Main(string[] args)
    {
        string root = FindProjectRoot();

        string sourcePath = Path.Combine(
            root, "Assets", "@Scripts"
        );

        string readmePath = Path.Combine(
            root, "README.md"
        );

        if (!Directory.Exists(sourcePath))
        {
            Console.WriteLine("Scripts 폴더를 찾을 수 없습니다.");
            return;
        }

        var files = Directory.GetFiles(
                sourcePath,
                "*.cs",
                SearchOption.AllDirectories
            )
            .OrderBy(file => file, StringComparer.Ordinal)
            .ToArray();

        var classesByFolder = new SortedDictionary<string, List<ClassDeclarationSyntax>>(
            StringComparer.Ordinal
        );

        foreach (var file in files)
        {
            string code = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(code);

            string relativeDirectory = Path.GetRelativePath(
                sourcePath,
                Path.GetDirectoryName(file)!
            );
            string folder = relativeDirectory == "."
                ? "Other"
                : relativeDirectory.Split(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar
                )[0];

            if (!classesByFolder.TryGetValue(folder, out var folderClasses))
            {
                folderClasses = new List<ClassDeclarationSyntax>();
                classesByFolder.Add(folder, folderClasses);
            }

            folderClasses.AddRange(
                tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
            );
        }

        var classes = classesByFolder.Values
            .SelectMany(folderClasses => folderClasses)
            .ToList();

        var classNames = classes
            .Select(c => c.Identifier.Text)
            .ToHashSet();

        var mermaid = new StringBuilder();

        // Mermaid 버전이 바뀌어도 이전 Dagre 배치를 유지한다.
        mermaid.AppendLine("---");
        mermaid.AppendLine("config:");
        mermaid.AppendLine("  layout: dagre");
        mermaid.AppendLine("  look: classic");
        mermaid.AppendLine("  themeCSS: '.cluster rect { fill: #888888 !important; fill-opacity: 0.22 !important; stroke: #888888 !important; }'");
        mermaid.AppendLine("---");
        mermaid.AppendLine("classDiagram");
        mermaid.AppendLine("direction TB");

        // 폴더별로 클래스를 묶어 관련 타입을 찾기 쉽게 한다.
        foreach (var folder in classesByFolder)
        {
            mermaid.AppendLine($"namespace ScriptsFolder_{folder.Key} {{");

            foreach (var c in folder.Value
                         .OrderBy(c => c.Identifier.Text, StringComparer.Ordinal))
            {
                string name = c.Identifier.Text;
                mermaid.AppendLine($"    class {name} {{");

                foreach (var attribute in c.AttributeLists
                                 .SelectMany(list => list.Attributes))
                {
                    mermaid.AppendLine(
                        $"        <<{FormatAttribute(attribute)}>>"
                    );
                }

                // 필드
                foreach (var field in c.Members
                                 .OfType<FieldDeclarationSyntax>())
                {
                    string visibility = GetVisibility(field.Modifiers);
                    string attributes = string.Join(
                        " ",
                        field.AttributeLists
                                .SelectMany(list => list.Attributes)
                                .Select(attribute => $"[{FormatAttribute(attribute)}]")
                    );
                    string modifiers = string.Join(
                        " ",
                        field.Modifiers
                                .Where(modifier => modifier.IsKind(SyntaxKind.StaticKeyword)
                                    || modifier.IsKind(SyntaxKind.ReadOnlyKeyword)
                                    || modifier.IsKind(SyntaxKind.ConstKeyword)
                                    || modifier.IsKind(SyntaxKind.VolatileKeyword)
                                    || modifier.IsKind(SyntaxKind.RequiredKeyword))
                                .Select(modifier => modifier.Text)
                    );

                    foreach (var variable in field.Declaration.Variables)
                    {
                        string initializer = variable.Initializer == null
                                ? string.Empty
                                : $" = {FormatMermaidText(variable.Initializer.Value.ToString())}";
                        string fieldAttributes = string.IsNullOrEmpty(attributes)
                                ? string.Empty
                                : $"{attributes} ";
                        string fieldModifiers = string.IsNullOrEmpty(modifiers)
                                ? string.Empty
                                : $"{modifiers} ";

                        mermaid.AppendLine(
                                $"        {visibility}{fieldAttributes}{fieldModifiers}{variable.Identifier.Text}: {FormatMermaidType(field.Declaration.Type)}{initializer}"
                        );
                    }
                }

                // 프로퍼티
                foreach (var property in c.Members
                             .OfType<PropertyDeclarationSyntax>())
                {
                    string visibility = GetVisibility(property.Modifiers);
                    string modifiers = string.Join(
                        " ",
                        property.Modifiers
                            .Where(modifier => modifier.IsKind(SyntaxKind.StaticKeyword)
                                || modifier.IsKind(SyntaxKind.VirtualKeyword)
                                || modifier.IsKind(SyntaxKind.OverrideKeyword)
                                || modifier.IsKind(SyntaxKind.AbstractKeyword)
                                || modifier.IsKind(SyntaxKind.SealedKeyword))
                            .Select(modifier => modifier.Text)
                    );
                    string accessors = property.AccessorList == null
                        ? string.Empty
                        : $" [{string.Join("; ", property.AccessorList.Accessors.Select(FormatAccessor))}]";
                    string initializer = property.Initializer == null
                        ? string.Empty
                        : $" = {FormatMermaidText(property.Initializer.Value.ToString())}";
                    string propertyModifiers = string.IsNullOrEmpty(modifiers)
                        ? string.Empty
                        : $"{modifiers} ";

                    mermaid.AppendLine(
                        $"        {visibility}{propertyModifiers}{property.Identifier.Text}: {FormatMermaidType(property.Type)}{accessors}{initializer}"
                    );
                }

                // 메서드
                foreach (var method in c.Members
                             .OfType<MethodDeclarationSyntax>())
                {
                    string visibility = GetVisibility(method.Modifiers);
                    string parameters = string.Join(
                        ", ",
                        method.ParameterList.Parameters.Select(p =>
                                p.Type == null
                                    ? p.Identifier.Text
                                    : $"{p.Identifier.Text}: {FormatMermaidType(p.Type)}"
                        )
                    );
                    string modifiers = string.Join(
                        ", ",
                        method.Modifiers
                            .Where(modifier => modifier.IsKind(SyntaxKind.VirtualKeyword)
                                || modifier.IsKind(SyntaxKind.OverrideKeyword)
                                || modifier.IsKind(SyntaxKind.AbstractKeyword)
                                || modifier.IsKind(SyntaxKind.SealedKeyword)
                                || modifier.IsKind(SyntaxKind.AsyncKeyword))
                            .Select(modifier => modifier.Text)
                    );
                    string modifierLabel = string.IsNullOrEmpty(modifiers)
                        ? string.Empty
                        : $" [{modifiers}]";
                    string staticMarker = method.Modifiers.Any(
                        modifier => modifier.IsKind(SyntaxKind.StaticKeyword)
                    )
                        ? "$"
                        : string.Empty;

                    mermaid.AppendLine(
                        $"        {visibility}{method.Identifier.Text}({parameters}) {FormatMermaidType(method.ReturnType)}{modifierLabel}{staticMarker}"
                    );
                }

                mermaid.AppendLine("    }");
            }

            mermaid.AppendLine("}");
        }

        var relationships = new Dictionary<
            (string Source, string Arrow, string Target),
            SortedSet<string>
        >();

        void AddRelationship(
            string source,
            string arrow,
            string target,
            string? label = null)
        {
            var key = (source, arrow, target);

            if (!relationships.TryGetValue(key, out var labels))
            {
                labels = new SortedSet<string>(StringComparer.Ordinal);
                relationships.Add(key, labels);
            }

            if (!string.IsNullOrEmpty(label))
                labels.Add(label);
        }

        // 1. 상속 관계
        foreach (var c in classes)
        {
            if (c.BaseList == null)
                continue;

            foreach (var baseType in c.BaseList.Types)
            {
                var references = ExtractClassReferences(
                    baseType.Type,
                    classNames
                );

                foreach (var parent in references)
                {
                    if (parent == c.Identifier.Text)
                        continue;

                    AddRelationship(parent, "<|--", c.Identifier.Text);
                }
            }
        }

        // 2. 필드 참조 관계
        foreach (var c in classes)
        {
            string className = c.Identifier.Text;

            foreach (var field in c.Members
                         .OfType<FieldDeclarationSyntax>())
            {
                var referencedTypes = ExtractClassReferences(
                    field.Declaration.Type,
                    classNames
                );

                foreach (var referencedType in referencedTypes)
                {
                    if (referencedType == className)
                        continue;

                    foreach (var variable in field.Declaration.Variables)
                    {
                        AddRelationship(
                            className,
                            "-->",
                            referencedType,
                            variable.Identifier.Text
                        );
                    }
                }
            }

            // 3. 프로퍼티 참조 관계
            foreach (var property in c.Members
                         .OfType<PropertyDeclarationSyntax>())
            {
                var referencedTypes = ExtractClassReferences(
                    property.Type,
                    classNames
                );

                foreach (var referencedType in referencedTypes)
                {
                    if (referencedType == className)
                        continue;

                    AddRelationship(
                        className,
                        "-->",
                        referencedType,
                        property.Identifier.Text
                    );
                }
            }

            // 4. 메서드 매개변수 및 반환 타입 관계
            foreach (var method in c.Members
                         .OfType<MethodDeclarationSyntax>())
            {
                var methodTypes = new List<TypeSyntax>();

                methodTypes.Add(method.ReturnType);

                methodTypes.AddRange(
                    method.ParameterList.Parameters
                        .Where(p => p.Type != null)
                        .Select(p => p.Type!)
                );

                foreach (var type in methodTypes)
                {
                    var referencedTypes = ExtractClassReferences(
                        type,
                        classNames
                    );

                    foreach (var referencedType in referencedTypes)
                    {
                        if (referencedType == className)
                            continue;

                        AddRelationship(
                            className,
                            "..>",
                            referencedType,
                            method.Identifier.Text
                        );
                    }
                }
            }

            // 5. 메서드 내부의 클래스 사용 관계
            foreach (var method in c.Members
                         .OfType<MethodDeclarationSyntax>())
            {
                if (method.Body == null &&
                    method.ExpressionBody == null)
                    continue;

                var body = method.Body != null
                    ? (SyntaxNode)method.Body
                    : method.ExpressionBody!.Expression;

                // GetComponent<T>()
                // FindFirstObjectByType<T>()
                // 기타 제네릭 메서드 호출
                foreach (var invocation in body
                             .DescendantNodes()
                             .OfType<InvocationExpressionSyntax>())
                {
                    if (invocation.Expression is not
                        MemberAccessExpressionSyntax memberAccess)
                        continue;

                    if (memberAccess.Name is not GenericNameSyntax generic)
                        continue;

                    string methodName = generic.Identifier.Text;

                    foreach (var argument in generic.TypeArgumentList.Arguments)
                    {
                        var referencedTypes = ExtractClassReferences(
                            argument,
                            classNames
                        );

                        foreach (var referencedType in referencedTypes)
                        {
                            if (referencedType == className)
                                continue;

                            AddRelationship(
                                className,
                                "..>",
                                referencedType,
                                methodName
                            );
                        }
                    }
                }

                // new SomeClass()
                foreach (var creation in body
                             .DescendantNodes()
                             .OfType<ObjectCreationExpressionSyntax>())
                {
                    var referencedTypes = ExtractClassReferences(
                        creation.Type,
                        classNames
                    );

                    foreach (var referencedType in referencedTypes)
                    {
                        if (referencedType == className)
                            continue;

                        AddRelationship(
                            className,
                            "..>",
                            referencedType,
                            "Instantiate"
                        );
                    }
                }
            }
        }

        // 관계 출력
        foreach (var relationship in relationships
                     .OrderBy(r => r.Key.Source, StringComparer.Ordinal)
                     .ThenBy(r => r.Key.Target, StringComparer.Ordinal)
                     .ThenBy(r => r.Key.Arrow, StringComparer.Ordinal))
        {
            string edge =
                $"{relationship.Key.Source} {relationship.Key.Arrow} {relationship.Key.Target}";

            if (relationship.Value.Count > 0)
                edge += $" : {string.Join(", ", relationship.Value)}";

            mermaid.AppendLine(edge);
        }

        // README 업데이트
        string generated = mermaid.ToString();

        if (!File.Exists(readmePath))
        {
            File.WriteAllText(readmePath, "# UnityStudy\n");
        }

        string readme = File.ReadAllText(readmePath);

        const string start = "<!-- UML_START -->";
        const string end = "<!-- UML_END -->";

        string section =
            $"{start}\n```mermaid\n{generated}```\n{end}";

        int startIndex = readme.IndexOf(start);
        int endIndex = readme.IndexOf(end);

        if (startIndex >= 0 && endIndex > startIndex)
        {
            readme = readme.Remove(
                startIndex,
                endIndex + end.Length - startIndex
            );

            readme = readme.Insert(startIndex, section);
        }
        else
        {
            readme += "\n## UML Class Diagram\n\n" + section;
        }

        File.WriteAllText(readmePath, readme);

        Console.WriteLine("UML 생성 완료!");
        Console.WriteLine($"분석 파일: {files.Length}개");
        Console.WriteLine($"클래스: {classes.Count}개");
        Console.WriteLine($"관계: {relationships.Sum(r => Math.Max(1, r.Value.Count))}개");
        Console.WriteLine($"관계선: {relationships.Count}개");
        Console.WriteLine("README.md 업데이트 완료!");
    }

    // 타입 내부에서 프로젝트 클래스 참조 추출
    static HashSet<string> ExtractClassReferences(
        TypeSyntax type,
        HashSet<string> classNames)
    {
        var references = new HashSet<string>();

        var identifiers = type
            .DescendantNodesAndSelf()
            .OfType<IdentifierNameSyntax>();

        foreach (var identifier in identifiers)
        {
            string name = identifier.Identifier.Text;

            if (classNames.Contains(name))
            {
                references.Add(name);
            }
        }

        return references;
    }

    static string GetVisibility(SyntaxTokenList modifiers)
    {
        if (modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)))
            return "+";

        if (modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ProtectedKeyword)))
            return "#";

        if (modifiers.Any(modifier => modifier.IsKind(SyntaxKind.InternalKeyword)))
            return "~";

        return "-";
    }

    static string FormatAccessor(AccessorDeclarationSyntax accessor)
    {
        string visibility = accessor.Modifiers.Count == 0
            ? string.Empty
            : $"{GetVisibility(accessor.Modifiers)} ";

        return $"{visibility}{accessor.Keyword.Text}";
    }

    static string FormatAttribute(AttributeSyntax attribute)
    {
        string arguments = attribute.ArgumentList == null
            ? string.Empty
            : $"({string.Join(", ", attribute.ArgumentList.Arguments.Select(argument => FormatMermaidText(argument.ToString())))})";

        return $"{attribute.Name}{arguments}";
    }

    static string FormatMermaidType(TypeSyntax type)
    {
        string text = type.ToString();
        var formatted = new StringBuilder(text.Length);
        int genericDepth = 0;

        foreach (char character in text)
        {
            if (character == '<')
            {
                genericDepth++;
                formatted.Append('~');
            }
            else if (character == '>' && genericDepth > 0)
            {
                genericDepth--;
                formatted.Append('~');
            }
            else if (character == ',' && genericDepth > 0)
            {
                formatted.Append(';');
            }
            else
            {
                formatted.Append(character);
            }
        }

        return formatted.ToString();
    }

    static string FormatMermaidText(string text)
    {
        return text
            .Replace('{', '[')
            .Replace('}', ']');
    }

    // Unity 프로젝트 루트 찾기
    static string FindProjectRoot()
    {
        var directory = new DirectoryInfo(
            Directory.GetCurrentDirectory()
        );

        while (directory != null)
        {
            if (Directory.Exists(
                Path.Combine(
                    directory.FullName,
                    "Assets",
                    "@Scripts"
                )
            ))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Unity 프로젝트 루트를 찾을 수 없습니다."
        );
    }
}