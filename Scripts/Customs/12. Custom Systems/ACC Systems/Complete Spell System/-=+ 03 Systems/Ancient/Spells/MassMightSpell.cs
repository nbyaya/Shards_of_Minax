using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Ancient
{
    public class AncientMassMightSpell : AncientSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Mass Might", "In Vas Por",
                                                        215,
                                                        9061,
                                                        Reagent.BlackPearl,
                                                        Reagent.MandrakeRoot,
                                                        Reagent.Ginseng
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public AncientMassMightSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
                Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                ArrayList targets = new ArrayList();

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 3);

                    foreach (Mobile m in eable)
                    {
                        if (Caster.CanBeBeneficial(m, false))
                            targets.Add(m);
                    }

                    eable.Free();
                }

                Effects.PlaySound(p, Caster.Map, 0x299);

                if (targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile targ = (Mobile)targets[i];

                        // AddStatBonus replaced by AddStatMod
                        StatMod strMod = new StatMod(StatType.Str, "MassMightStr", 10, TimeSpan.FromMinutes(2));
                        StatMod dexMod = new StatMod(StatType.Dex, "MassMightDex", 10, TimeSpan.FromMinutes(2));
                        StatMod intMod = new StatMod(StatType.Int, "MassMightInt", 10, TimeSpan.FromMinutes(2));

                        targ.AddStatMod(strMod);
                        targ.AddStatMod(dexMod);
                        targ.AddStatMod(intMod);

                        targ.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                        targ.PlaySound(0x1EA);
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AncientMassMightSpell m_Owner;

            public InternalTarget(AncientMassMightSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
