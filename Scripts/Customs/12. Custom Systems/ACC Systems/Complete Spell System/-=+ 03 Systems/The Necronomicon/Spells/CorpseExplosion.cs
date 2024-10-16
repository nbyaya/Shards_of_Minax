using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class CorpseExplosion : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Corpse Explosion", "Corpore Vilem",
            //SpellCircle.Fourth,
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public CorpseExplosion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CorpseExplosion m_Owner;

            public InternalTarget(CorpseExplosion owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Corpse)
                {
                    m_Owner.Target((Corpse)targeted);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }
        }

        public void Target(Corpse corpse)
        {
            if (!Caster.CanSee(corpse))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                // Consume scroll if needed
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, corpse);

                Effects.PlaySound(corpse.Location, Caster.Map, 0x208); // Explosion sound

                // Visual explosion effect
                Effects.SendLocationEffect(corpse.Location, Caster.Map, 0x36BD, 20, 10, 0, 0);

                // Damage and apply plague effect to nearby enemies
                ArrayList targets = new ArrayList();
                foreach (Mobile m in corpse.GetMobilesInRange(3))
                {
                    if (Caster != m && Caster.CanBeHarmful(m, false))
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    // Apply damage
                    int damage = Utility.RandomMinMax(20, 40);
                    Caster.DoHarmful(m);
                    AOS.Damage(m, Caster, damage, 0, 100, 0, 0, 0);

                    // Apply plague effect
                    m.SendMessage("You have been weakened by the plague from the exploding corpse!");
                    m.PlaySound(0x1F1); // Plague sound
                    m.ApplyPoison(Caster, Poison.Lethal);
                }

                corpse.Delete(); // Remove the exploded corpse
            }

            FinishSequence();
        }
    }
}
