using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class GroundSlam : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Ground Slam", "Terra Quassare",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public GroundSlam(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Effects on caster
                Caster.PlaySound(0x2E6);
                Caster.Animate(12, 5, 1, true, false, 0); // Animates a strong attack animation
                Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x36BD, 20, 10, 1160, 0); // Ground impact effect

                // Define the shockwave area and effects
                Map map = Caster.Map;
                if (map == null)
                    return;

                ArrayList targets = new ArrayList();
                IPooledEnumerable eable = Caster.GetMobilesInRange(2); // 2-tile radius around the caster
                foreach (Mobile m in eable)
                {
                    if (m != Caster && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }
                eable.Free();

                // Apply effects to each target
                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    Caster.DoHarmful(m);
                    Effects.SendTargetEffect(m, 0x37B9, 10, 20, 1160, 0); // Shockwave visual effect on target

                    // Knockback effect
                    int dx = m.X - Caster.X;
                    int dy = m.Y - Caster.Y;

                    if (dx != 0 || dy != 0)
                    {
                        int dist = 1 + (int)(Caster.Skills[SkillName.Macing].Value / 20); // Distance depends on caster's skill
                        Point3D newLocation = new Point3D(m.X + dist * Math.Sign(dx), m.Y + dist * Math.Sign(dy), m.Z);

                        if (map.CanFit(newLocation, 16, true, false))
                        {
                            m.Location = newLocation;
                            m.ProcessDelta();
                        }
                    }

                    // Additional effects like stun or damage can be added here
                    m.Damage(10, Caster); // Apply some damage
                    m.SendMessage("You are knocked back by the shockwave!");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
