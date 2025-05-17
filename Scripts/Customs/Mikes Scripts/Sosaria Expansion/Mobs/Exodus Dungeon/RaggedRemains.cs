using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("the ragged remains of a tormented soul")]
    public class RaggedRemains : BaseCreature
    {
        private DateTime m_NextSoulLeech;
        private DateTime m_NextCorpseBind;
        private DateTime m_NextReconstruct;

        [Constructable]
        public RaggedRemains()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Ragged Remains";
            Body = 309;
            BaseSoundID = 0x48D;
            Hue = 2954;

            SetStr(700, 850);
            SetDex(90, 120);
            SetInt(250, 320);

            SetHits(1000, 1200);

            SetDamage(22, 30);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 105.0);
            SetSkill(SkillName.SpiritSpeak, 120.0);
            SetSkill(SkillName.Necromancy, 120.0);

            Fame = 18000;
            Karma = -22000;

            VirtualArmor = 75;

            m_NextSoulLeech = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextCorpseBind = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextReconstruct = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 4;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextSoulLeech)
                    SoulLeech();

                if (DateTime.UtcNow >= m_NextCorpseBind)
                    CorpseBind();

                if (DateTime.UtcNow >= m_NextReconstruct && Hits < (HitsMax / 2))
                    SelfReconstruct();
            }
        }

        private void SoulLeech()
        {
            if (Combatant is Mobile target)
            {
                target.SendMessage(0x22, "* Your soul is drained by the Ragged Remains! *");
                int drain = Utility.RandomMinMax(25, 50);
                AOS.Damage(target, this, drain, 0, 0, 0, 0, 100); // Energy drain
                Heal(drain / 2);
                FixedParticles(0x375A, 10, 15, 5036, EffectLayer.Head);
                PlaySound(0x1F2);
            }

            m_NextSoulLeech = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CorpseBind()
        {
            foreach (Item item in GetItemsInRange(8))
            {
                if (item is Corpse corpse && !corpse.Deleted && corpse.Owner != this)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ragged Remains binds nearby corpses to rise! *");

                    Mobile bound = new BoundRemnant();
                    bound.MoveToWorld(corpse.Location, corpse.Map);
                    corpse.Delete();

                    Effects.SendLocationParticles(bound, 0x37B9, 10, 20, 5042);
                    PlaySound(0x48D);
                }
            }

            m_NextCorpseBind = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void SelfReconstruct()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Ragged Remains stitches its shattered bones together... *");
            PlaySound(0x29D);
            Heal(Utility.RandomMinMax(100, 200));
            Effects.SendLocationParticles(this, 0x376A, 9, 32, 5042);
            m_NextReconstruct = DateTime.UtcNow + TimeSpan.FromSeconds(90);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (Utility.RandomDouble() < 0.1 && from != null)
            {
                from.SendMessage(0x22, "* You feel your strength fading into the Ragged Remains... *");
                from.Stam -= Utility.Random(5, 10);
                from.Mana -= Utility.Random(5, 10);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, 4);
        }

        public RaggedRemains(Serial serial) : base(serial) { }

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

    public class BoundRemnant : Skeleton
    {

        public BoundRemnant()
        {
            Name = "a bound remnant";
            Hue = 2412; // Pale blue undead glow
            SetStr(50);
            SetDex(40);
            SetInt(10);
            SetHits(40);
            SetDamage(5, 10);


            Timer.DelayCall(TimeSpan.FromSeconds(20), Delete);
        }

        public BoundRemnant(Serial serial) : base(serial) { }

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
