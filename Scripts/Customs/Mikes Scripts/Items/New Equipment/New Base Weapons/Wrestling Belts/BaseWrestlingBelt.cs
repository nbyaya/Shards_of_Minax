using System;
using Server;

namespace Server.Items
{
    public abstract class BaseWrestlingBelt : BaseMeleeWeapon
    {
        public BaseWrestlingBelt(int itemID)
            : base(itemID)
        {
            Layer = Layer.OneHanded; // Same slot as LeatherNinjaBelt
        }

        public BaseWrestlingBelt(Serial serial) : base(serial) { }

        public override SkillName DefSkill => SkillName.Wrestling;

        public override WeaponType DefType => WeaponType.Bashing;

        public override WeaponAnimation DefAnimation => WeaponAnimation.Bash1H;

        public override int DefHitSound => 0x23B;

        public override int DefMissSound => 0x238;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
