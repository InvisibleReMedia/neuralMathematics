namespace Interfaces
{

    /// <summary>
    /// Neurone interface declaration
    /// Déclare une interface commune à tous les neurones
    /// </summary>
    public interface INeurone
    {
        /// <summary>
        /// Nom du neurone
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Identifiant unique
        /// </summary>
        uint Id { get; }
        /// <summary>
        /// Type du neurone
        /// </summary>
        INeuroneType Type { get; }
        /// <summary>
        /// Exécute le travail du neurone
        /// </summary>
        void Exec();
        /// <summary>
        /// Apprentissage pour la construction
        /// des neurones
        /// </summary>
        void Learn();
    }
}
