using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("a nimogwai corpse")]
    public class Nimogwai : BaseCreature
    {
        private DateTime _NextNatureWrath;
        private DateTime _NextBindingVines;

        [Constructable]
        public Nimogwai()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Nimogwai";
            Body = 240;
            Hue = 1166;
            BaseSoundID = 0x508;

            SetStr(500, 600);
            SetDex(100, 150);
            SetInt(150, 200);

            SetMana(200);
            SetHits(1200, 1500);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 40;

            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems, 3);

            PackItem(new IronOre(Utility.RandomMinMax(10, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new ArcaneGem());
            if (Utility.RandomDouble() < 0.005)
                PackItem(new BarkFragment(5));
        }

        public Nimogwai(Serial serial)
            : base(serial)
        {
        }

        public override int GetAngerSound() { return 0x50B; }
        public override int GetIdleSound()  { return 0x50A; }
        public override int GetAttackSound(){ return 0x509; }
        public override int GetHurtSound()  { return 0x50C; }
        public override int GetDeathSound() { return 0x508; }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var target = Combatant;
            if (target != null && !target.Deleted && target.Map == Map && InRange(target, 8) 
                && CanBeHarmful(target) && InLOS(target) 
                && DateTime.UtcNow >= _NextNatureWrath)
            {
				var mobileTarget = target as Mobile;
				if (mobileTarget != null)
				{
					CastNatureWrath(mobileTarget);
					_NextNatureWrath = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
				}
            }
        }

		public void CastNatureWrath(Mobile target)
		{
			this.Animate(Utility.RandomList(10, 11), 5, 1, true, false, 0);
			this.PlaySound(0x50B);

			// Wind‑up particles on the target
			Effects.SendTargetParticles((Mobile)target, 0x376A, 9, 32, 0x13B9, 0, 0, EffectLayer.Waist, 0);

			Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
			{
				if (target == null || target.Deleted || target.Map != Map)
					return;

				this.PlaySound(0x22F);

				// Explosion at the location
				Effects.SendLocationParticles(
					EffectItem.Create(target.Location, target.Map, TimeSpan.FromSeconds(0.5)), 
					0x3709, 10, 30, 0x13B2, 0, 0x1391, 0);

				var nearby = target.GetMobilesInRange(3);
				foreach (Mobile m in nearby)
				{
					if (m != this && CanBeHarmful(m))
					{
						DoHarmful(m);

						int raw = Utility.RandomMinMax(20, 40);
						int physRes = m.PhysicalResistance;
						int dmg = (int)(raw * (1.0 - physRes / 100.0));
						dmg = Math.Max(1, dmg);

						AOS.Damage(m, this, dmg, 50, 0, 25, 0, 25);

						if (Utility.RandomDouble() < 0.5)
						{
							m.SendLocalizedMessage(1004016);
							StatMod smod = new StatMod(StatType.Str, "NimogwaiStrDebuff", -Utility.RandomMinMax(10, 20), TimeSpan.FromSeconds(10));
							m.AddStatMod(smod);

							// Debuff particles
							Effects.SendTargetParticles((Mobile)m, 0x374A, 10, 15, 5013, 1, 0, EffectLayer.Waist, 0);
						}
					}
				}
				nearby.Free();
			});
		}


        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender != null && !defender.Deleted 
                && DateTime.UtcNow >= _NextBindingVines 
                && Utility.RandomDouble() < 0.3)
            {
                DoHarmful(defender);
                defender.SendLocalizedMessage(1070819);
                defender.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                defender.PlaySound(0x20F);

                defender.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5)));

                // Create and store the mod so we can remove it properly
                int reduce = Utility.RandomMinMax(5, 10);
                var rmod = new ResistanceMod(ResistanceType.Physical, -reduce);
                defender.AddResistanceMod(rmod);

                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () =>
                {
                    defender.RemoveResistanceMod(rmod);
                });

                _NextBindingVines = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override int Meat { get { return 3; } }

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
