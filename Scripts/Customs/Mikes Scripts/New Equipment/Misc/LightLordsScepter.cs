using System;
using Server;

namespace Server.Items
{
    public class LightLordsScepter : Pitchfork
    {
        public override int InitMinHits { get { return 225; } }
        public override int InitMaxHits { get { return 225; } }
        public override int ArtifactRarity { get { return 14; } }

        [Constructable]
        public LightLordsScepter()
        {
            Name = "The Light Lord's Scepter";
            Hue = 1150; // Contrasting hue
            Attributes.WeaponDamage = -50; // This can represent a healing effect
            WeaponAttributes.HitColdArea = 100; // Increased cold area effect
            WeaponAttributes.ResistColdBonus = 5;
            Attributes.SpellChanneling = 0; // Prevents spell channeling
            Attributes.WeaponSpeed = 25; // Increased speed
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = 0;
            cold = 100; // 100% cold damage
            fire = 0;
            nrgy = 0;
            pois = 0;
            chaos = 0;
            direct = 0;
        }

        public LightLordsScepter(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}