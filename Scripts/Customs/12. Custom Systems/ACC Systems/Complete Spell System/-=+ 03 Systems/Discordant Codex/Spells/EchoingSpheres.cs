using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class EchoingSpheres : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Echoing Spheres", "Voce Sphaera",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public EchoingSpheres(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    List<Point3D> positions = GetRandomPositions(Caster.Location, Caster.Map, 5, 5);

                    foreach (Point3D position in positions)
                    {
                        Item sphere = new SphereItem();
                        sphere.MoveToWorld(position, Caster.Map);
                        Effects.SendLocationParticles(EffectItem.Create(sphere.Location, sphere.Map, EffectItem.DefaultDuration), 0x373A, 1, 32, 0x47D, 4, 9502, 0);
                        Effects.PlaySound(sphere.Location, sphere.Map, 0x208);

                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => ApplyEffects(sphere, 1));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in Echoing Spheres: " + ex.Message);
                }
            }

            FinishSequence();
        }

        private void ApplyEffects(Item sphere, int range)
        {
            if (sphere.Deleted) return;

            foreach (Mobile m in sphere.GetMobilesInRange(range))
            {
                if (m != null && m != Caster && m.Alive && !m.IsDeadBondedPet)
                {
                    if (m.Player || (m is BaseCreature && !((BaseCreature)m).BardImmune))
                    {
                        int effect = -20; // Example effect value
                        List<StatMod> mods = new List<StatMod>
                        {
                            new StatMod(StatType.Str, "EchoingSpheresStr", effect, TimeSpan.FromSeconds(10)),
                            new StatMod(StatType.Dex, "EchoingSpheresDex", effect, TimeSpan.FromSeconds(10)),
                            new StatMod(StatType.Int, "EchoingSpheresInt", effect, TimeSpan.FromSeconds(10))
                        };

                        foreach (var mod in mods)
                        {
                            m.AddStatMod(mod);
                        }

                    }
                }
            }

            // Schedule the next application of effects
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => ApplyEffects(sphere, range));
        }

        private List<Point3D> GetRandomPositions(Point3D origin, Map map, int radius, int count)
        {
            List<Point3D> points = new List<Point3D>();

            for (int i = 0; i < count; i++)
            {
                int x = origin.X + Utility.RandomMinMax(-radius, radius);
                int y = origin.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);

                points.Add(new Point3D(x, y, z));
            }

            return points;
        }
    }

    public class SphereItem : Item
    {
        public SphereItem() : base(0x1F1F)
        {
            Movable = false;
            Hue = 0x47D; // Blueish color for visual effect
        }

        public SphereItem(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => this.Delete());
        }
    }
}
