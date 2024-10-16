using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections;


namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class ZestyDistraction : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Zesty Distraction", "Rattus Invictus!",
            21011,
            9307
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 17; } }

        public ZestyDistraction(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Summon multiple rats around the caster
                    int numberOfRats = Utility.RandomMinMax(3, 6); // Random number of rats between 3 and 6
                    for (int i = 0; i < numberOfRats; i++)
                    {
                        BaseCreature rat = new Rat();
                        int x = Caster.X + Utility.RandomMinMax(-2, 2);
                        int y = Caster.Y + Utility.RandomMinMax(-2, 2);
                        int z = Caster.Map.GetAverageZ(x, y); // Adjust Z coordinate to the terrain

                        // Set rat properties
                        rat.Controlled = false; // Not controlled by the caster
                        rat.Summoned = true;
                        rat.SummonMaster = Caster; // Set the caster as the summoner
                        rat.Map = Caster.Map; // Set rat map to caster's map
                        rat.Location = new Point3D(x, y, z); // Set the location of the rat

                        // Add rat to the world
                        rat.MoveToWorld(new Point3D(x, y, z), Caster.Map);

                        // Schedule the rat to be deleted after 60 seconds
                        Timer.DelayCall(TimeSpan.FromSeconds(60.0), () =>
                        {
                            if (rat != null && rat.Alive)
                                rat.Delete(); // Remove the rat from the world
                        });

                        rat.PlaySound(0xCC); // Rat squeak sound
                        Effects.SendLocationEffect(new Point3D(x, y, z), Caster.Map, 0x3728, 10, 1, 1153, 0); // A green smoke effect
                    }

                    // Play a distraction sound and visual effect at the caster's location
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x206); // Sparkle sound
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 10, 1, 1153, 0); // Flashy effect

                }
                catch (Exception ex)
                {
                    Caster.SendMessage("Something went wrong while casting the spell."); // Error handling
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(4.0);
        }
    }
}
