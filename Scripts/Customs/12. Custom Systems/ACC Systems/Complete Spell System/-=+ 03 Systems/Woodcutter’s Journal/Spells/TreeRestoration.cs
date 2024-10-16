using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class TreeRestoration : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tree Restoration", // Name of the spell
            "Uus Ailem",       // Spell text
            21004,             // Spell graphics ID
            9304,              // Spell sound ID
            false,             // Is spell an area effect
            Reagent.MandrakeRoot, // First reagent
            Reagent.Garlic      // Replace Reagent.SpringWater with a valid reagent like Reagent.Garlic
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public TreeRestoration(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TreeRestoration m_Owner;

            public InternalTarget(TreeRestoration owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
                else
                    from.SendMessage("You must target a tree or a tree stump.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // Visual effects and sounds
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(p), Caster.Map, EffectItem.DefaultDuration), 0x373A, 10, 15, 1153, 0, 0, 0);
                Effects.PlaySound(p, Caster.Map, 0x5C3);

                // Logic for restoring the tree
                Item targetItem = new Item(Utility.RandomList(0x0CE3, 0x0CE4)); // Tree types
                targetItem.MoveToWorld(new Point3D(p), Caster.Map);

                Timer.DelayCall(TimeSpan.FromMinutes(5.0), () => RegenerateTree(targetItem, p, Caster.Map));

                Caster.SendMessage("You restore vitality to the tree, causing it to heal and grow.");
            }

            FinishSequence();
        }

        private void RegenerateTree(Item tree, IPoint3D p, Map map)
        {
            if (tree == null || tree.Deleted) return;

            // Regenerate resources
            tree.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "The tree produces fresh resources.");
            Effects.SendLocationParticles(EffectItem.Create(new Point3D(p), map, EffectItem.DefaultDuration), 0x373A, 10, 15, 1153, 0, 0, 0);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
