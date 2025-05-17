using System;
using Server;
using Server.Items;
using Server.Spells; // Needed for StatMod

namespace Server.Mobiles
{
    [CorpseName("a decaying lizard corpse")]
    public class DecayingLizard : BaseCreature
    {
        private DateTime _NextNoxiousCloud;
        private DateTime _NextDecaySpit;

        [Constructable]
        public DecayingLizard()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a decaying lizard";
            Body = 0xCE;        // Same body ID as Lava Lizard
            BaseSoundID = 0x5A; // Same base sound ID as Lava Lizard
            Hue = 2101;         // A sickly grey-green hue for decay

            // --- Significantly Increased Stats ---
            SetStr(350, 450);
            SetDex(80, 100);
            SetInt(30, 50);

            SetHits(450, 550);
            SetMana(100, 150);

            SetDamage(15, 30);

            // --- Damage Types & Resistances ---
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50); // Adds poison damage

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 20, 35);    // Decaying flesh might burn easier
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 70, 85);  // Highly resistant to poison
            SetResistance(ResistanceType.Energy, 40, 55);

            // --- Skills ---
            SetSkill(SkillName.MagicResist, 90.0, 105.0);
            SetSkill(SkillName.Tactics, 90.0, 105.0);
            SetSkill(SkillName.Wrestling, 90.0, 105.0);
            SetSkill(SkillName.Anatomy, 75.0, 90.0);
            SetSkill(SkillName.Poisoning, 100.0, 120.0); // Master poisoner

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 55; // Higher inherent armor
        }

        public DecayingLizard(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel { get { return true; } } // Can dispel summons/fields
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison
        public override Poison HitPoison { get { return Poison.Lethal; } } // Applies lethal poison on hit

        public override int Hides { get { return 18; } }
        public override HideType HideType { get { return HideType.Spined; } } // Keeping spined hides
        public override int Meat { get { return 3; } } // Slightly more meat

        // --- Ability Implementation ---

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            // Use IDamageable for Combatant
            IDamageable combatant = Combatant;

            // Check if combatant is valid and Mobile before using Mobile-specific properties/methods
            if (combatant is Mobile target)
            {
                if (target != null && !target.Deleted && target.Alive && target.Map == Map && CanBeHarmful(target))
                {
                    // Try Noxious Cloud first if ready and target is close enough
                    if (DateTime.UtcNow >= _NextNoxiousCloud && InRange(target, 6) && InLOS(target))
                    {
                        EmitNoxiousCloud(target);
                        _NextNoxiousCloud = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25)); // Cooldown 15-25 sec
                        _NextDecaySpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)); // Delay spit after cloud
                    }
                    // Otherwise, try Decay Spit if ready and target is in range
                    else if (DateTime.UtcNow >= _NextDecaySpit && InRange(target, 10) && !InRange(target, 1) && InLOS(target)) // Prefers spitting at range
                    {
                        SpitDecay(target);
                        _NextDecaySpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)); // Cooldown 10-20 sec
                    }
                }
            }
        }

        public void EmitNoxiousCloud(Mobile primaryTarget)
        {
            this.PlaySound(0x230); // Noxious gas sound
            this.Animate(AnimationType.Attack, 7); // Attack animation

            // Visual effect at the lizard's location
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, 5052); // Greenish cloud effect

            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "*Emits a cloud of choking decay!*");

            // Affect mobiles in range
            foreach (Mobile mob in this.GetMobilesInRange(4)) // Affect targets within 4 tiles
            {
                if (mob != null && mob != this && CanBeHarmful(mob))
                {
                    DoHarmful(mob);

                    // Direct Poison Damage
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(mob, this, damage, 0, 0, 0, 100, 0); // 100% Poison damage

                    // Apply debuff using StatMod for temporary stat reduction
                    if (mob is Mobile targetMobile)
                    {
                        targetMobile.SendMessage(0x22, "You choke on the noxious fumes, feeling weaker!");
                        TimeSpan duration = TimeSpan.FromSeconds(10);

                        targetMobile.AddStatMod(new StatMod(StatType.Str, "NoxiousCloudStr", -20, duration));
                        targetMobile.AddStatMod(new StatMod(StatType.Int, "NoxiousCloudInt", -20, duration));

                        // Optional: Add a visual effect for the debuff
                        targetMobile.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                    }
                }
            }
        }

        public void SpitDecay(Mobile target)
        {
            this.PlaySound(0x108); // Spit sound
            this.Animate(AnimationType.Attack, 7); // Attack animation

            DoHarmful(target);

            this.MovingParticles(target, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160); // Green goo particle effect

            // Delay damage/poison application slightly to match particle travel time
            Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
            {
                if (target != null && target.Alive && CanBeHarmful(target))
                {
                    if (target is Mobile mobileTarget)
                    {
                        mobileTarget.PlaySound(0x1D); // Impact sound

                        // Direct Damage (Physical/Poison mix)
                        int damage = Utility.RandomMinMax(15, 25);
                        AOS.Damage(mobileTarget, this, damage, 50, 0, 0, 50, 0); // 50% Physical, 50% Poison

                        // Apply Lethal Poison
                        mobileTarget.ApplyPoison(this, Poison.Lethal);
                        mobileTarget.SendMessage(0x22, "Searing decay burns your skin!"); // Red message
                    }
                }
            });
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Withering Touch ability - 30% chance on hit
            if (0.3 > Utility.RandomDouble())
            {
                if (defender is Mobile mobileDefender)
                {
                    int staminaDrain = Utility.RandomMinMax(15, 30);
                    int manaDrain = Utility.RandomMinMax(10, 25);

                    // Use 'Stam' instead of 'Stamina'
                    if (mobileDefender.Stam >= staminaDrain)
                    {
                        mobileDefender.Stam -= staminaDrain;
                        mobileDefender.SendMessage(0x22, "The lizard's touch drains your energy!");
                        this.PlaySound(0x1F9); // Drain sound
                    }
                    if (mobileDefender.Mana >= manaDrain)
                    {
                        mobileDefender.Mana -= manaDrain;
                    }

                    // Optional visual effect on the defender
                    mobileDefender.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                }
            }
        }

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Potions, 1);

            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));

            if (Utility.RandomDouble() < 0.1)
                PackItem(new LethalPoisonPotion());
        }

        // --- Standard Serialization ---
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
    }
}
