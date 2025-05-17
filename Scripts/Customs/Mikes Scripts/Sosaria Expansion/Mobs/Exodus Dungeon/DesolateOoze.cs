using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a desolate slime")]
    public class DesolateOoze : BaseCreature
    {
        private DateTime m_NextCorrode;
        private DateTime m_NextMindLeech;
        private DateTime m_NextOozeDuplicate;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public DesolateOoze() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Desolate Ooze";
            Body = 780; // Same body as Bog Thing
            BaseSoundID = 352;
            Hue = 2965; // Sickly dark green and purple swirl hue

            SetStr(900, 1050);
            SetDex(60, 80);
            SetInt(200, 250);

            SetHits(1100, 1400);
            SetMana(1000);

            SetDamage(20, 35);
            SetDamageType(ResistanceType.Poison, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Poisoning, 100.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 60;
            m_AbilitiesInitialized = false;
        }

        public override bool BleedImmune => true;
        public override Poison HitPoison => Poison.Lethal;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool BardImmune => true;
        public override bool AutoDispel => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 4);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_AbilitiesInitialized)
            {
                m_NextCorrode = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(5, 15));
                m_NextMindLeech = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 30));
                m_NextOozeDuplicate = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 45));
                m_AbilitiesInitialized = true;
            }

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextCorrode)
                    CorrosiveSpray();

                if (DateTime.UtcNow >= m_NextMindLeech)
                    MindLeech();

                if (DateTime.UtcNow >= m_NextOozeDuplicate)
                    AttemptDuplicate();
            }
        }

        private void CorrosiveSpray()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*The Desolate Ooze expels a wave of corrosive slime!*");
            PlaySound(0x231);

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m == this || !m.Alive)
                    continue;

                AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 0, 0, 0, 100);
                m.SendMessage(0x22, "You are burned by caustic slime!");
                m.ApplyPoison(this, Poison.Greater);
            }

            m_NextCorrode = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

		private void MindLeech()
		{
			if (Combatant is Mobile target && target.Alive)
			{
				PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*A tendril of psychic slime lashes toward your mind!*");
				PlaySound(0x1E1);

				Effects.SendBoltEffect(target);

				target.Mana -= Utility.Random(10, 30);
				target.Stam -= Utility.Random(5, 20);
				target.SendMessage(0x22, "Your mind is drained by the ooze!");

				m_NextMindLeech = DateTime.UtcNow + TimeSpan.FromSeconds(30);
			}
		}


        private void AttemptDuplicate()
        {
            if (Hits > (HitsMax * 0.5) && Utility.RandomDouble() < 0.2)
            {
                DesolateSlime clone = new DesolateSlime(this);
                clone.MoveToWorld(Location, Map);

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*A portion of the ooze breaks off and animates!*");
                PlaySound(0x229);
            }

            m_NextOozeDuplicate = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        public DesolateOoze(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class DesolateSlime : BaseCreature
    {
        public DesolateSlime(DesolateOoze parent)
            : base(AIType.AI_Melee, FightMode.Closest, 6, 1, 0.3, 0.5)
        {
            Name = "a slime spawn";
            Body = 780;
            BaseSoundID = 352;
            Hue = parent.Hue;
            SetStr(100);
            SetDex(50);
            SetInt(10);

            SetHits(75);
            SetDamage(5, 10);

            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Poison, 100);
            SetSkill(SkillName.Wrestling, 30.0);
            SetSkill(SkillName.Tactics, 30.0);

            VirtualArmor = 10;
        }

        public DesolateSlime(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
