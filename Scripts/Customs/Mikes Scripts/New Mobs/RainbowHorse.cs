using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Custom.Mobiles
{
    public class RainbowHorse : BaseMount
    {
        // List of hues for cycling
        private static int[] hues = new int[] { 1153, 2406, 1266, 1265, 1266, 1267, 1268, 1269, 1175, 1281 };
        
        [Constructable]
        public RainbowHorse() : base("a rainbow horse", 200, 0x3E90, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            // Initial hue
            Hue = hues[0];
            ControlSlots = 1;
            Tamable = true;
            MinTameSkill = 29.1;
            SetStr(96, 120);
            SetDex(56, 80);
            SetInt(6, 10);
            SetHits(58, 72);
            SetDamage(3, 7);
            SetDamageType(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Physical, 15, 20);

            // Start the hue changing timer
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5), CycleHue);
        }

        private int currentHueIndex = 0;

        public void CycleHue()
        {
            currentHueIndex = (currentHueIndex + 1) % hues.Length;
            this.Hue = hues[currentHueIndex];
        }

        public RainbowHorse(Serial serial) : base(serial)
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
            // Restart the hue cycling when deserialized
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5), CycleHue);
        }
    }
}