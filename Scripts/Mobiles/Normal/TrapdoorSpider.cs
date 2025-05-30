using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a trapdoor spider corpse")]
    public class TrapdoorSpider : BaseCreature
    {
        public override bool CanStealth { get { return true; } } 

        [Constructable]
        public TrapdoorSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a trapdoor spider";
            Body = 737;
            Hidden = true; 

            SetStr(100, 104);
            SetDex(162, 165);
            SetInt(29, 50);

            SetHits(125, 144);

            SetDamage(15, 18);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 0);
            SetResistance(ResistanceType.Fire, 30, 35);
            SetResistance(ResistanceType.Cold, 30, 35);
            SetResistance(ResistanceType.Poison, 40, 45);
            SetResistance(ResistanceType.Energy, 95, 100);

            SetSkill(SkillName.Anatomy, 2.0, 3.8);
            SetSkill(SkillName.MagicResist, 47.5, 57.9);
            SetSkill(SkillName.Poisoning, 70.5, 73.5);
            SetSkill(SkillName.Tactics, 73.3, 78.9);
            SetSkill(SkillName.Wrestling, 92.5, 94.6);
            SetSkill(SkillName.Hiding, 110.3, 119.9);
            SetSkill(SkillName.Stealth, 110.5, 119.6);
        }

        public TrapdoorSpider(Serial serial)
            : base(serial)
        {
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            RevealingAction();
            base.OnDamage(amount, from, willKill);
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            RevealingAction();
            base.OnDamagedBySpell(from);
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 2;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new SerpentmastersGauntlets());
            }			
        }

        public override int GetIdleSound()
        {
            return 1605;
        }

        public override int GetAngerSound()
        {
            return 1602;
        }

        public override int GetHurtSound()
        {
            return 1604;
        }

        public override int GetDeathSound()
        {
            return 1603;
        }

        public override void OnThink()
        {

            if (!this.Alive || this.Deleted)
            {
                return;
            }

            if (!this.Hidden)
            {
                double chance = 0.05;

                if (this.Hits < 20)
                {
                    chance = 0.1;
                }

                if (this.Poisoned)
                {
                    chance = 0.01;
                }

                if (Utility.RandomDouble() < chance)
                {
                    HideSelf();
                }
                base.OnThink();
            }
        }

        private void HideSelf()
        {
            if (Core.TickCount >= this.NextSkillTime)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                this.PlaySound(0x22F);
                this.Hidden = true;

                this.UseSkill(SkillName.Stealth);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}