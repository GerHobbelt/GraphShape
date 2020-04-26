﻿using System;
using System.Windows;
using System.Collections.Generic;
using GraphShape.Algorithms.Layout;
using GraphShape.Algorithms.Layout.Simple.Hierarchical;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph;
using static GraphShape.Tests.Algorithms.AlgorithmTestHelpers;

namespace GraphShape.Tests.Algorithms.Layout
{
    /// <summary>
    /// Tests related to <see cref="EfficientSugiyamaLayoutAlgorithm{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class EfficientSugiyamaLayoutTests : LayoutAlgorithmTestBase
    {
        [Test]
        public void Constructor()
        {
            var verticesPositions = new Dictionary<string, Point>();
            var verticesSizes = new Dictionary<string, Size>();
            var graph = new BidirectionalGraph<string, Edge<string>>();
            var algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph);
            algorithm.IterationEnded += (sender, args) => { };
            AssertAlgorithmProperties(algorithm, graph, expectedReportIterationEnd: true);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph);
            algorithm.ProgressChanged += (sender, args) => { };
            AssertAlgorithmProperties(algorithm, graph, expectedReportProgress: true);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph);
            algorithm.IterationEnded += (sender, args) => { };
            algorithm.ProgressChanged += (sender, args) => { };
            AssertAlgorithmProperties(algorithm, graph, expectedReportIterationEnd: true, expectedReportProgress: true);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, verticesPositions, null);
            AssertAlgorithmProperties(algorithm, graph, verticesPositions);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, verticesSizes);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, verticesPositions, verticesSizes);
            AssertAlgorithmProperties(algorithm, graph, verticesPositions);

            var parameters = new EfficientSugiyamaLayoutParameters();
            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, parameters);
            AssertAlgorithmProperties(algorithm, graph, parameters: parameters);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, null, parameters);
            AssertAlgorithmProperties(algorithm, graph, parameters: parameters);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, verticesPositions, null, parameters);
            AssertAlgorithmProperties(algorithm, graph, verticesPositions, parameters: parameters);

            algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(graph, verticesPositions, verticesSizes, parameters);
            AssertAlgorithmProperties(algorithm, graph, verticesPositions, parameters: parameters);
        }

        [Test]
        public void Constructor_Throws()
        {
            var verticesPositions = new Dictionary<string, Point>();
            var verticesSizes = new Dictionary<string, Size>();
            var parameters = new EfficientSugiyamaLayoutParameters();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(null, parameters));
            Assert.Throws<ArgumentNullException>(
                () => new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(null, verticesSizes, parameters));
            Assert.Throws<ArgumentNullException>(
                () => new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(null, verticesPositions, verticesSizes, parameters));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> EfficientSugiyamaLayoutTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new BidirectionalGraph<string, Edge<string>>(), 0, 0)
                {
                    TestName = "Empty graph"
                };

                var graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVertex("0");
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Single vertex graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "0"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Single vertex self loop graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVertexRange(new[] { "0", "1" });
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Two vertices graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Two linked vertices graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVertex("2");
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Three vertices graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Four vertices graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "1"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Multiple vertices self loop graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "2"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "4"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "5"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Line graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "2"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "4"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "5"));
                graph.AddVerticesAndEdge(new Edge<string>("5", "1"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Cycle line graph"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "2"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "4"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "5"));
                yield return new TestCaseData(graph, 0, 0)
                {
                    TestName = "Cycle graph"
                };

                IBidirectionalGraph<string, Edge<string>> completeGraph = GraphFactory.CreateCompleteGraph(
                    7,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t));
                yield return new TestCaseData(completeGraph, 40, 0)
                {
                    TestName = "Complete graph"
                };

                IBidirectionalGraph<string, Edge<string>> tree = GraphFactory.CreateTree(
                    20,
                    2,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(tree, 5, 4)
                {
                    TestName = "Tree graph 20 vertices/2 branches"
                };
                
                tree = GraphFactory.CreateTree(
                    25,
                    5,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(tree, 0, 0)
                {
                    TestName = "Tree graph 25 vertices/5 branches"
                };

                IBidirectionalGraph<string, Edge<string>> dag = GraphFactory.CreateDAG(
                    25,
                    25,
                    10,
                    10,
                    true,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(dag, 15, 0)
                {
                    TestName = "DAG graph 25 vertices/25 edges (Parallel edge)"
                };

                IBidirectionalGraph<string, Edge<string>> generalGraph = GraphFactory.CreateGeneralGraph(
                    30,
                    15,
                    10,
                    true,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(generalGraph, 0, 0)
                {
                    TestName = "Generic graph 30 vertices/15 edges (Parallel edge)"
                };

                IBidirectionalGraph<string, Edge<string>> isolatedVerticesGraph = GraphFactory.CreateIsolatedVerticesGraph<string, Edge<string>>(
                    15,
                    i => i.ToString());
                yield return new TestCaseData(isolatedVerticesGraph, 0, 0)
                {
                    TestName = "Isolated vertices graph (15 vertices)"
                };
            }
        }

        [TestCaseSource(nameof(EfficientSugiyamaLayoutTestCases))]
        public void EfficientSugiyamaLayoutAlgorithm(
            [NotNull] IBidirectionalGraph<string, Edge<string>> graph,
            int maxCrossCount,
            int maxOverlapped)
        {
            IDictionary<string, Size> verticesSizes = GetVerticesSizes(graph.Vertices);

            var parameters = new EfficientSugiyamaLayoutParameters();

            foreach (LayoutDirection direction in Enum.GetValues(typeof(LayoutDirection)))
            {
                parameters.Direction = direction;

                foreach (int mode in new[] { -1, 0, 1, 2, 3 })
                {
                    parameters.PositionMode = mode;

                    foreach (bool optimizeWidth in new[] { false, true })
                    {
                        parameters.OptimizeWidth = optimizeWidth;

                        foreach (bool minimizeEdgeLength in new[] { false, true })
                        {
                            parameters.MinimizeEdgeLength = minimizeEdgeLength;
                    
                            var algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(
                                graph,
                                verticesSizes,
                                parameters)
                            {
                                Rand = new Random(12345)
                            };

                            LayoutResults results = ExecuteLayoutAlgorithm(algorithm, verticesSizes);
                            results.CheckResult(maxCrossCount, maxOverlapped);
                        }
                    }
                }
            }
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> EfficientSugiyamaLayoutEdgeRoutingTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new BidirectionalGraph<string, Edge<string>>())
                {
                    TestName = "Empty graph (edge routing)"
                };

                var graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVertex("0");
                yield return new TestCaseData(graph)
                {
                    TestName = "Single vertex graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "0"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Single vertex self loop graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVertexRange(new[] { "0", "1" });
                yield return new TestCaseData(graph)
                {
                    TestName = "Two vertices graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Two linked vertices graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVertex("2");
                yield return new TestCaseData(graph)
                {
                    TestName = "Three vertices graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Four vertices graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "1"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Multiple vertices self loop graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "2"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "4"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "5"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Line graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "2"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "4"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "5"));
                graph.AddVerticesAndEdge(new Edge<string>("5", "1"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Cycle line graph (edge routing)"
                };

                graph = new BidirectionalGraph<string, Edge<string>>();
                graph.AddVerticesAndEdge(new Edge<string>("0", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("1", "2"));
                graph.AddVerticesAndEdge(new Edge<string>("2", "3"));
                graph.AddVerticesAndEdge(new Edge<string>("3", "4"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "1"));
                graph.AddVerticesAndEdge(new Edge<string>("4", "5"));
                yield return new TestCaseData(graph)
                {
                    TestName = "Cycle graph (edge routing)"
                };

                IBidirectionalGraph<string, Edge<string>> completeGraph = GraphFactory.CreateCompleteGraph(
                    7,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t));
                yield return new TestCaseData(completeGraph)
                {
                    TestName = "Complete graph (edge routing)"
                };

                IBidirectionalGraph<string, Edge<string>> tree = GraphFactory.CreateTree(
                    20,
                    2,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(tree)
                {
                    TestName = "Tree graph 20 vertices/2 branches (edge routing)"
                };

                tree = GraphFactory.CreateTree(
                    25,
                    5,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(tree)
                {
                    TestName = "Tree graph 25 vertices/5 branches (edge routing)"
                };

                IBidirectionalGraph<string, Edge<string>> dag = GraphFactory.CreateDAG(
                    25,
                    25,
                    10,
                    10,
                    true,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(dag)
                {
                    TestName = "DAG graph 25 vertices/25 edges (Parallel edge) (edge routing)"
                };

                IBidirectionalGraph<string, Edge<string>> generalGraph = GraphFactory.CreateGeneralGraph(
                    30,
                    15,
                    10,
                    true,
                    i => i.ToString(),
                    (s, t) => new Edge<string>(s, t),
                    new Random(123));
                yield return new TestCaseData(generalGraph)
                {
                    TestName = "Generic graph 30 vertices/15 edges (Parallel edge) (edge routing)"
                };

                IBidirectionalGraph<string, Edge<string>> isolatedVerticesGraph = GraphFactory.CreateIsolatedVerticesGraph<string, Edge<string>>(
                    15,
                    i => i.ToString());
                yield return new TestCaseData(isolatedVerticesGraph)
                {
                    TestName = "Isolated vertices graph (15 vertices) (edge routing)"
                };
            }
        }

        [TestCaseSource(nameof(EfficientSugiyamaLayoutEdgeRoutingTestCases))]
        public void EfficientSugiyamaLayoutAlgorithmEdgeRouting([NotNull] IBidirectionalGraph<string, Edge<string>> graph)
        {
            IDictionary<string, Size> verticesSizes = GetVerticesSizes(graph.Vertices);

            var parameters = new EfficientSugiyamaLayoutParameters();

            foreach (SugiyamaEdgeRouting routing in Enum.GetValues(typeof(SugiyamaEdgeRouting)))
            {
                parameters.EdgeRouting = routing;
                var algorithm = new EfficientSugiyamaLayoutAlgorithm<string, Edge<string>, IBidirectionalGraph<string, Edge<string>>>(
                    graph,
                    verticesSizes,
                    parameters)
                {
                    Rand = new Random(12345)
                };

                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Assert.DoesNotThrow(() => ExecuteLayoutAlgorithm(algorithm, verticesSizes));
            }
        }
    }
}