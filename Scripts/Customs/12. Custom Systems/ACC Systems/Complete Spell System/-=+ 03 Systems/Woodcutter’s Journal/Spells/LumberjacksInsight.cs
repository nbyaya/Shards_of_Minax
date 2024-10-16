using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class LumberjacksInsight : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Lumberjack’s Insight", "Fet Gott",
            21008, 9308
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public LumberjacksInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private LumberjacksInsight m_Owner;

            public InternalTarget(LumberjacksInsight owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.RevealResources((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void RevealResources(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            SpellHelper.Turn(Caster, p);

            // Add a visual effect and sound when casting the spell
            Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x373A, 30, 10, 1153, 0);
            Effects.PlaySound(p, Caster.Map, 0x1E9);

            // Reveal hidden resources logic
            List<Item> items = new List<Item>();
            foreach (Item item in Caster.Map.GetItemsInRange(new Point3D(p), 5))
            {
                if (item is Log || item is Board)
                {
                    items.Add(item);
                    item.Visible = true;
                }
            }

            if (items.Count > 0)
            {
                Caster.SendMessage("You reveal hidden resources in the area!");
            }
            else
            {
                Caster.SendMessage("No hidden resources found.");
            }

            FinishSequence();
        }
    }

    public abstract class WoodcuttersJournalSpell : Spell
    {
        public WoodcuttersJournalSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public override bool ClearHandsOnCast { get { return false; } }
        public override bool RevealOnCast { get { return true; } }
    }
}
