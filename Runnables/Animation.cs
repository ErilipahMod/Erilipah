using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Erilipah.Runnables
{
    public abstract class Animation : Runnable
    {
        protected readonly SpriteBatch spriteBatch;

        public int TargetClient { get; }
        public bool ProvidesImmunity { get; protected set; }

        protected Animation(SpriteBatch spriteBatch, int targetClient)
        {
            this.spriteBatch = spriteBatch;
            TargetClient = targetClient;
        }

        protected Animation(SpriteBatch spriteBatch) : this(spriteBatch, Main.myPlayer) { }

        public static Animation Quick(SpriteBatch spriteBatch, int targetClient, bool providesImmunity, Action<SpriteBatch> draw)
        {
            return new QuickAnimation(spriteBatch, targetClient, providesImmunity, draw);
        }

        public override void Run()
        {
            if (Main.myPlayer == TargetClient)
            {
                Main.blockMouse = true;
                Main.blockInput = true;
            }
            if (ProvidesImmunity)
            {
                Main.player[TargetClient].immune = true;
                Main.player[TargetClient].immuneTime = 30;
                Main.player[TargetClient].immuneNoBlink = true;
            }
        }

        public override void OnEnd()
        {
            if (Main.myPlayer == TargetClient)
            {
                Main.blockMouse = false;
                Main.blockInput = false;
            }
        }

        private class QuickAnimation : Animation
        {
            private readonly Action<SpriteBatch> draw;

            public QuickAnimation(SpriteBatch spriteBatch, int targetClient, bool providesImmunity, Action<SpriteBatch> draw) : base(spriteBatch, targetClient)
            {
                ProvidesImmunity = providesImmunity;
                this.draw = draw;
            }

            public override void Run()
            {
                base.Run();

                draw(spriteBatch);
            }
        }
    }
}
