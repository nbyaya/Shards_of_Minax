using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.SkillHandlers;

namespace Server.Mobiles
{
    [CorpseName("a crystal changeling corpse")]
    public class CrystalChangeling : BaseCreature
    {
        private Mobile m_MorphedInto;
        private DateTime m_LastMorph;
        private DateTime m_NextCrystalSurge;
        private DateTime m_NextMirrorSpike;
        private DateTime m_NextReflectiveShift;

        [Constructable]
        public CrystalChangeling()
            : base(AIType.AI_Spellweaving, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Crystal Changeling";
            Body = 264;
            Hue = 1153;
            BaseSoundID = 0x46E;

            SetStr(250, 350);
            SetDex(200, 250);
            SetInt(400, 500);

            SetHits(1000, 1300);
            SetStam(250, 350);
            SetMana(600, 800);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.Spellweaving, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 60;
        }

        public CrystalChangeling(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override bool BardImmune => true;
        public override bool Unprovokable => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 4);
            PackItem(new CrystalFragment());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (m_MorphedInto != target && Utility.RandomDouble() < 0.03)
                {
                    Morph(target);
                    m_LastMorph = DateTime.UtcNow;
                }

                if (DateTime.UtcNow >= m_NextCrystalSurge)
                    CrystalSurge(target);

                if (DateTime.UtcNow >= m_NextMirrorSpike)
                    MirrorSpike(target);

                if (DateTime.UtcNow >= m_NextReflectiveShift)
                    ReflectiveShift();
            }

            if (m_MorphedInto != null && DateTime.UtcNow - m_LastMorph > TimeSpan.FromSeconds(45))
            {
                Revert();
            }
        }

        // === Ability 1: Crystal Surge ===
        private void CrystalSurge(Mobile target)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Crystal Changeling channels a shard surge!*");
            PlaySound(0x652);

            AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 0, 0, 100); // Pure energy damage

            if (Utility.RandomDouble() < 0.5)
            {
                target.Freeze(TimeSpan.FromSeconds(2));
                target.SendMessage(0x22, "You're temporarily frozen by crystalline energy!");
            }

            m_NextCrystalSurge = DateTime.UtcNow + TimeSpan.FromSeconds(30 + Utility.RandomMinMax(5, 10));
        }

        // === Ability 2: Mirror Spike ===
        private void MirrorSpike(Mobile target)
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "*Crystalline spikes burst from mirrored space!*");
            PlaySound(0x64D);

            Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 20, 10);
            AOS.Damage(target, this, Utility.RandomMinMax(15, 30), 100, 0, 0, 0, 0); // Physical burst

            m_NextMirrorSpike = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.RandomMinMax(5, 8));
        }

        // === Ability 3: Reflective Shift (temporary displacement) ===
        private void ReflectiveShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Crystal Changeling fractures and reforms!*");
            PlaySound(0x511);
            FixedParticles(0x376A, 1, 14, 5045, EffectLayer.Waist);

            // Teleport within 5 tiles randomly
            int dx = Utility.RandomMinMax(-5, 5);
            int dy = Utility.RandomMinMax(-5, 5);
            Point3D newLoc = new Point3D(X + dx, Y + dy, Map.GetAverageZ(X + dx, Y + dy));

            if (Map.CanFit(newLoc, 16, false, false))
            {
                Location = newLoc;
            }

            m_NextReflectiveShift = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        protected virtual void Morph(Mobile target)
        {
            Body = target.Body;
            Hue = 1153;
            Name = "Crystal " + target.Name;

            foreach (Item item in target.Items)
            {
                if (item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank)
                    AddItem(new ClonedItem(item));
            }

            PlaySound(0x511);
            FixedParticles(0x376A, 1, 14, 5045, EffectLayer.Waist);
            m_MorphedInto = target;
        }

        protected virtual void Revert()
        {
            Body = 264;
            Hue = 1153;
            Name = "a Crystal Changeling";

            DeleteClonedItems();
            PlaySound(0x511);
            FixedParticles(0x376A, 1, 14, 5045, EffectLayer.Waist);

            m_MorphedInto = null;
        }

        private void DeleteClonedItems()
        {
            for (int i = Items.Count - 1; i >= 0; --i)
            {
                if (Items[i] is ClonedItem)
                    Items[i].Delete();
            }
        }

        public override int GetAngerSound() => 0x46E;
        public override int GetIdleSound() => 0x470;
        public override int GetAttackSound() => 0x46D;
        public override int GetHurtSound() => 0x471;
        public override int GetDeathSound() => 0x46F;

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

        private class ClonedItem : Item
        {
            public ClonedItem(Item item) : base(item.ItemID)
            {
                Name = item.Name;
                Weight = item.Weight;
                Hue = item.Hue;
                Layer = item.Layer;
                Movable = false;
            }

            public ClonedItem(Serial serial) : base(serial) { }

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

    public class CrystalFragment : Item
    {
        [Constructable]
        public CrystalFragment() : base(0x1F19)
        {
            Name = "a crystal fragment";
            Hue = 1153;
            Weight = 1.0;
        }

        public CrystalFragment(Serial serial) : base(serial) { }

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
