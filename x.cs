public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
{
    var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
        .Append(mlContext.Transforms.Text.FeaturizeText("TextFeatures", "Text"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding("LocationEncoded", "Location"))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding("TimeOfDayEncoded", "TimeOfDay"))
        .Append(mlContext.Transforms.Concatenate("Features", "TextFeatures", "LocationEncoded", "TimeOfDayEncoded"))
        .Append(mlContext.MulticlassClassification.Trainers.TextClassification(labelColumnName:"Label", sentence1ColumnName: "Text")) 
        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

    return pipeline;
}
